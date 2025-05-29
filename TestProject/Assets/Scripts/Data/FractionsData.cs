using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Содержит данные характерные фракций сортированых по типу.
    /// </summary>
    public sealed class FractionsData
    {
        public readonly Dictionary<int, FractionData> allFractionsData = new Dictionary<int, FractionData>();
    }
}
