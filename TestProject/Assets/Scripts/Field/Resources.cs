using Services;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace View
{
    public class Resources : MonoBehaviourLogger
    {
        [SerializeField] private Resource resourcePrototype = null;
        /// <summary>
        /// Половина стороны квадрата для появления ресурсов.
        /// </summary>
        [SerializeField, Tooltip("Половина стороны квадрата для появления ресурсов.")]
        private float halfSideSqureSpawnZone = 40f;
        [SerializeField] private float resourceSpawnRate = 3f;
        private SettingsDataService settingsDataService;

        private void OnDataChanged()
        {
            resourceSpawnRate = settingsDataService.resourceSpawnRate;
        }

        public void Initialize(ServicesContainer container)
        {
            settingsDataService = container.settingsDataService;
            settingsDataService.dataChanged += OnDataChanged;
        }
        public readonly List<Resource> resources = new List<Resource>();
        private float lastSpawnTime = -1f;
        private void Update()
        {
            if (lastSpawnTime + resourceSpawnRate < Time.time)
            {
                lastSpawnTime = Time.time;
                Resource newResource = null;
                foreach (Resource resource in resources)
                {
                    if (!resource.isReady)
                    {
                        newResource = resource;
                    }
                }

                //Реализация аля пулл, полноценный тут не нужен.
                if (newResource == null)
                {
                    newResource = Instantiate(resourcePrototype);
                    newResource.transform.parent = transform;
                }

                newResource.isReady = true;
                newResource.transform.localPosition = new Vector3
                    (
                    Random.Range(-halfSideSqureSpawnZone, halfSideSqureSpawnZone),
                    0,
                    Random.Range(-halfSideSqureSpawnZone, halfSideSqureSpawnZone)
                    );

                resources.Add(newResource);
            }
        }
        protected override void OnDestroy()
        {
            settingsDataService.dataChanged -= OnDataChanged;
            base.OnDestroy();
        }
    }
}