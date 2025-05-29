using Services;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace View
{
    public class ComandCenter : MonoBehaviourLogger
    {
        [SerializeField] private Combine combinePrototype = null;
        [SerializeField] private MaterialSetter materialSetter = null;
        [SerializeField] private Transform startPoint = null;
        [SerializeField] private ParticleSystem[] addResourceParticleSystems = new ParticleSystem[0];
        [SerializeField] private float combineCreatingRate = 2f;
        [SerializeField] private float updateRate = 0.1f;
        public int combineMax = 5;

        private int team;
        private readonly List<Combine> combines = new List<Combine>();
        private List<Combine> allCombines;
        private List<Resource> targets;
        private float lastCreatedCombineTime = -1f;
        private float lastUpdateTime;
        private ServicesContainer servicesContainer;

        private void OnResourceInBaseColleted()
        {
            foreach (ParticleSystem system in addResourceParticleSystems)
            {
                if (!system.isPlaying)
                {
                    system.Play();
                    break;
                }
            }
            servicesContainer.fractionsDataService.AddResources(team, 1);
        }
        private void CreateCombine()
        {
            Combine newCombine = null;
            foreach (Combine combine in combines)
            {
                if (!combine.isActiveCombine)
                    newCombine = combine;
            }

            //Реализация аля пулл, полноценный тут не нужен.
            if (newCombine == null)
            {
                newCombine = Instantiate(combinePrototype);
                newCombine.resourceInBaseColleted += OnResourceInBaseColleted;
                newCombine.transform.parent = transform;
                combines.Add(newCombine);
                allCombines.Add(newCombine);
                newCombine.Initialize(targets, combines, team, startPoint, allCombines, allCombines.Count, servicesContainer);
            }

            newCombine.isActiveCombine = true;
            newCombine.transform.position = startPoint.position;
        }

        public void Initialize(List<Resource> targets, List<Combine> allCombines, int team, ServicesContainer servicesContainer)
        {
            this.servicesContainer = servicesContainer;
            this.allCombines = allCombines;
            this.team = team;
            this.targets = targets;
            materialSetter.SetMaterial(team);
        }
        private int GetActiveCombine()
        {
            int count = 0;
            foreach (Combine combine in combines)
                if (combine.isActiveCombine) ++count;
            return count;
        }
        public void UpdateCC(float allTime)
        {
            if (lastUpdateTime + updateRate < allTime)
            {
                lastUpdateTime = allTime;
                int activeCombines = GetActiveCombine();
                bool isTimeForCreate = lastCreatedCombineTime + combineCreatingRate < allTime;
                bool isNotMax = activeCombines < servicesContainer.settingsDataService.dronsCount;
                bool isHaveFreeResource = activeCombines < targets.Count;
                if (isTimeForCreate && isNotMax && isHaveFreeResource)
                {
                    lastCreatedCombineTime = allTime;
                    CreateCombine();
                }
            }

            foreach (Combine combine in combines)
            {
                if (combine.isActiveCombine)
                {
                    combine.CombineUpdate(allTime);
                }
            }
        }

        private void Awake()
        {
            SetLogPrefix(nameof(ComandCenter));

            IsNullCheck(combinePrototype, nameof(combinePrototype));
            IsNullCheck(materialSetter, nameof(materialSetter));
        }
    }
}