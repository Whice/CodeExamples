using Common;
using Data;
using System;

namespace Services
{
    /// <summary>
    /// Пердоставляет доступ к необходимым данным для их изменения или чтения.
    /// </summary>
    public sealed class SettingsDataService : LoggableObject
    {
        private SettingsData data;

        public int dronsCount
        {
            get => data.dronsCount;
            set
            {
                if (data.dronsCount != value)
                {
                    if (value < 1) value = 1;
                    data.dronsCount = value;
                    dataChanged?.Invoke();
                }
            }
        }
        public float dronsSpeed
        {
            get => data.dronsSpeed;
            set
            {
                if (value < 1) value = 1;
                data.dronsSpeed = value;
                dataChanged?.Invoke();
            }
        }
        public float resourceSpawnRate
        {
            get => data.resourceSpawnRate;
            set
            {
                if (value <= 0) value = 1;
                data.resourceSpawnRate = value;
                dataChanged?.Invoke();
            }
        }
        public bool isShowTrail
        {
            get => data.isShowTrail;
            set
            {
                data.isShowTrail = value;
                dataChanged?.Invoke();
            }
        }

        public event Action dataChanged;
        public void Initialize(SettingsData fractionsData)
        {
            IsNullCheck(fractionsData, nameof(fractionsData));

            data = fractionsData;
        }
    }
}
