using Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace View
{
    public class Combine : MonoBehaviourLogger
    {
        [SerializeField] private NavMeshAgent agent = null;
        [SerializeField] private float timeForCollect = 2f;
        [SerializeField] private float distanceForCollect = 3f;
        [SerializeField] private float distanceForDeactivateInBase = 3f;
        [SerializeField] private float updateRate = 0.1f;
        [SerializeField] private float distanceForWorkaround = 10f;
        [SerializeField] private GameObject collectedResourceSignal = null;
        [SerializeField] private MaterialSetter materialSetter = null;
        [SerializeField] private GameObject combineTrail = null;

        private int team;
        private List<Resource> targets;
        private List<Combine> teamCombines;
        private List<Combine> allCombines;
        private Resource currentTarget;
        public bool isActiveCombine
        {
            get => gameObject.activeSelf;
            set => SetActiveObject(value);
        }
        private float startCollect;
        private float lastUpdateTime;
        private bool isCollected = false;
        private Transform startPoint;
        private int priority;
        private SettingsDataService settingsDataService;
        private enum State
        {
            NONE,
            MOVE_TO_TARGET,
            COLLECT,
            MOVE_TO_BASE
        }
        private State currentState = State.NONE;
        /// <summary>
        /// Ресурс был доставлен на базу.
        /// </summary>
        public event Action resourceInBaseColleted;


        private void OnDronDataChanged()
        {
            agent.speed = settingsDataService.dronsSpeed;
            combineTrail.SetActive(settingsDataService.isShowTrail);
        }
        private void OnReadyTargetChanged(Resource r)
        {
            r.readyChanged -= OnReadyTargetChanged;
            if (isCollected)
            {
                currentTarget = null;
                agent.SetDestination(startPoint.position);
            }
            else if (!currentTarget.isReady)
            {
                FindTarget();
            }
        }
        private void SetTarget(Resource target)
        {
            if (target == null)
            {
                //Подождать и найти цель позже.
                currentState = State.NONE;
            }
            else
            {
                currentTarget = target;
                currentTarget.readyChanged += OnReadyTargetChanged;
                agent.SetDestination(currentTarget.transform.position);
                currentState = State.MOVE_TO_TARGET;
            }
        }
        private float GetSqrDistance(Transform t1, Transform t2)
        {
            return (t1.position - t2.position).sqrMagnitude;
        }
        private float GetSqrDistance(MonoBehaviour mb1, MonoBehaviour mb2)
        {
            return GetSqrDistance(mb1.transform, mb2.transform);
        }
        private void FindTarget()
        {
            List<Resource> availableResources = new List<Resource>();
            foreach (Resource target in targets)
            {
                if (target.isReady)
                {
                    bool targetFreeForTeam = true;
                    foreach (Combine combine in teamCombines)
                    {
                        if (combine.currentTarget == target)
                        {
                            targetFreeForTeam = false;
                            break;
                        }
                    }
                    if (targetFreeForTeam)
                    {
                        availableResources.Add(target);
                    }
                }
            }
            Resource newTarget = null;
            if (availableResources.Count > 0)
            {
                newTarget = availableResources[0];
                float minDistance = GetSqrDistance(availableResources[0], this);
                foreach (Resource target in availableResources)
                {
                    float distance = GetSqrDistance(target, this);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        newTarget = target;
                    }
                }
            }
            SetTarget(newTarget);
        }
        /// <summary>
        /// Проверить доступность целевого ресурса, переназначить, если текущий недоступен.
        /// </summary>
        private bool CheckCurrentTargetReady()
        {
            if (currentTarget == null || !currentTarget.isReady)
            {
                FindTarget();
                return false;
            }
            else
            {
                return true;
            }
        }
        public void CombineUpdate(float allTime)
        {
            if (lastUpdateTime + updateRate < allTime)//Не нужно слишком часто вычислять.
            {
                float availableDistance = distanceForWorkaround * distanceForWorkaround;
                Combine nearestCombine = null;
                float minDistance = float.MaxValue;
                foreach (Combine combine in allCombines)
                {
                    bool isNotThis = combine != this;
                    float distance = GetSqrDistance(this, combine);
                    bool isDistanceSmall = availableDistance > distance;
                    bool isLowPriority = combine.priority > priority;
                    if (isNotThis && isDistanceSmall && isLowPriority && combine.isActiveCombine)
                    {
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestCombine = combine;
                        }
                    }
                }

                if (nearestCombine == null)
                {
                    if (currentState == State.MOVE_TO_TARGET && currentTarget != null)
                        agent.SetDestination(currentTarget.transform.position);
                    else if (currentState == State.MOVE_TO_BASE)
                        agent.SetDestination(startPoint.position);
                }
                else
                {
                    //Нужно больше уточнений, чтобы система уклонений работала лучше.
                    //Можно, например, снижать скорость, когда поблиззости есть другие корабли.
                    Vector3 scaledDirection = transform.position - nearestCombine.transform.position;
                    agent.SetDestination(transform.position + (scaledDirection * 15));
                }

                if (currentState == State.NONE)
                {
                    FindTarget();
                }

                if (currentState == State.MOVE_TO_TARGET)
                {
                    if (CheckCurrentTargetReady())
                    {
                        lastUpdateTime = allTime;
                        float sqrDistanceForCollect = distanceForCollect * distanceForCollect;
                        float sqrDistanceToTarget = (currentTarget.transform.position - transform.position).sqrMagnitude;
                        if (sqrDistanceForCollect > sqrDistanceToTarget)
                        {
                            currentState = State.COLLECT;
                            startCollect = allTime;
                        }
                    }
                }

                if (currentState == State.COLLECT && startCollect + timeForCollect < allTime)
                {
                    if (CheckCurrentTargetReady())
                    {
                        collectedResourceSignal.SetActive(true);
                        currentState = State.MOVE_TO_BASE;
                        isCollected = true;
                        currentTarget.isReady = false;
                    }
                }

                if (currentState == State.MOVE_TO_BASE)
                {
                    float sqrDistanceForDeactivate = distanceForDeactivateInBase * distanceForDeactivateInBase;
                    float sqrDistanceToBase = (startPoint.position - transform.position).sqrMagnitude;
                    if (sqrDistanceForDeactivate > sqrDistanceToBase)
                    {
                        collectedResourceSignal.SetActive(false);
                        isActiveCombine = false;
                        resourceInBaseColleted?.Invoke();
                        currentState = State.NONE;
                    }
                }
            }
        }
        public void Initialize(List<Resource> targets, List<Combine> combines, int team, Transform startPoint, List<Combine> allCombines, int priority, ServicesContainer servicesContainer)
        {
            settingsDataService = servicesContainer.settingsDataService;
            settingsDataService.dataChanged += OnDronDataChanged;
            this.allCombines = allCombines;
            this.priority = priority;
            collectedResourceSignal.SetActive(false);
            this.startPoint = startPoint;
            this.targets = targets;
            teamCombines = combines;
            this.team = team;
            FindTarget();
            materialSetter.SetMaterial(team);
            OnDronDataChanged();
        }
        private void Awake()
        {
            SetLogPrefix(nameof(Combine));

            IsNullCheck(agent, nameof(agent));
            IsNullCheck(collectedResourceSignal, nameof(collectedResourceSignal));
            IsNullCheck(materialSetter, nameof(materialSetter));
            IsNullCheck(combineTrail, nameof(combineTrail));
        }
        protected override void OnDestroy()
        {
            settingsDataService.dataChanged -= OnDronDataChanged;
            base.OnDestroy();
        }
    }
}