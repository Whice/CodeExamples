using Common;
using Data;
using System;

namespace Services
{
    /// <summary>
    /// Пердоставляет доступ к необходимым данным для их изменения или чтения.
    /// </summary>
    public sealed class FractionsDataService : LoggableObject
    {
        private FractionsData data;

        public event Action dataChanged;
        public void Initialize(FractionsData fractionsData)
        {
            data = fractionsData;
            AddResources(0, 0);
            AddResources(1, 0);
        }
        public void AddResources(int fractionNumber, int count)
        {
            if (!data.allFractionsData.ContainsKey(fractionNumber))
            {
                data.allFractionsData[fractionNumber] = new FractionData();
            }
            data.allFractionsData[fractionNumber].resourcesCount += count;
            dataChanged?.Invoke();
        }
        public int GetResourcesCount(int fractionNumber)
        {
            if (data.allFractionsData.ContainsKey(fractionNumber))
            {
                return data.allFractionsData[fractionNumber].resourcesCount;
            }
            else
            {
                LogError($"Fraction type {fractionNumber} not found!");
                return -1;
            }
        }
    }
}
