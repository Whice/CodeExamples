using Data;
using Utility;

namespace Services
{
    public sealed class ServicesContainer
    {
        private DataContainer data;

        public readonly FractionsDataService fractionsDataService = new FractionsDataService();
        public readonly SettingsDataService settingsDataService = new SettingsDataService();

        public void Initialize(DataContainer dataContainer)
        {
            data = dataContainer;

            fractionsDataService.SetLoggerWithPrefix(new SimpleLogger(), nameof(FractionsDataService));
            fractionsDataService.Initialize(dataContainer.fractionsData);
            settingsDataService.SetLoggerWithPrefix(new SimpleLogger(), nameof(SettingsDataService));
            settingsDataService.Initialize(dataContainer.settingsData);
        }
    }
}
