using Services;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace View
{
    public class GameField : MonoBehaviourLogger
    {
        [SerializeField] private Resources resources = null;
        [SerializeField] private ComandCenter ComandCenter1 = null;
        [SerializeField] private ComandCenter ComandCenter2 = null;


        private readonly List<Combine> allCombines = new List<Combine>();
        public void Initialize(ServicesContainer servicesContainer)
        {
            ComandCenter1.Initialize(resources.resources, allCombines, 0, servicesContainer);
            ComandCenter2.Initialize(resources.resources, allCombines, 1, servicesContainer);
            resources.Initialize(servicesContainer);
        }
        private void Update()
        {
            float allTime = Time.time;
            ComandCenter1.UpdateCC(allTime);
            ComandCenter2.UpdateCC(allTime);
        }
        private void Awake()
        {
            SetLogPrefix(nameof(GameField));

            IsNullCheck(resources, nameof(resources));
            IsNullCheck(ComandCenter1, nameof(ComandCenter1));
            IsNullCheck(ComandCenter2, nameof(ComandCenter2));
        }
    }
}