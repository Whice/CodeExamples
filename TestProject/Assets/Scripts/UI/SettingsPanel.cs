using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class SettingsPanel : MonoBehaviourLogger
    {
        [SerializeField] private Slider simulationSpeed = null;
        [SerializeField] private Button toNormalSimulationSpeedButton = null;
        [SerializeField] private TextMeshProUGUI dronsCountText = null;
        [SerializeField] private Slider dronsCount = null;
        [SerializeField] private TextMeshProUGUI dronsSpeedText = null;
        [SerializeField] private Slider dronsSpeed = null;
        [SerializeField] private TMP_InputField resourceSpawnRate = null;
        [SerializeField] private Toggle showTrails = null;

        private SettingsDataService settingsDataService;
        private void DataUpdate()
        {
            settingsDataService.dronsCount = (int)dronsCount.value;
            dronsCountText.text = $"Drons count: {settingsDataService.dronsCount}";

            settingsDataService.dronsSpeed = dronsSpeed.value;
            dronsSpeedText.text = $"Drons speed: {settingsDataService.dronsSpeed}";

            if (resourceSpawnRate.text.TryParseToFloat(out float spawnRate))
            {
                settingsDataService.resourceSpawnRate = spawnRate;
            }
            else
            {
                settingsDataService.resourceSpawnRate = 1;
                LogError($"Error converting to a number for {nameof(resourceSpawnRate)}!");
            }

            settingsDataService.isShowTrail = showTrails.isOn;
        }

        public void Initialize(SettingsDataService service)
        {
            IsNullCheck(service, nameof(FractionsDataService));

            settingsDataService = service;

            dronsCount.onValueChanged.AddListener((value) => DataUpdate());
            dronsSpeed.onValueChanged.AddListener((value) => DataUpdate());
            resourceSpawnRate.onValueChanged.AddListener((value) => DataUpdate());
            showTrails.onValueChanged.AddListener((value) => DataUpdate());
            DataUpdate();

            simulationSpeed.onValueChanged.AddListener((value) => Time.timeScale = value);
            toNormalSimulationSpeedButton.onClick.AddListener(() => simulationSpeed.value = 1);
        }
        private void Awake()
        {

            SetLogPrefix(nameof(SettingsPanel));

            IsNullCheck(dronsCountText, nameof(dronsCountText));
            IsNullCheck(dronsCount, nameof(dronsCount));
            IsNullCheck(dronsSpeedText, nameof(dronsSpeedText));
            IsNullCheck(dronsSpeed, nameof(dronsSpeed));
            IsNullCheck(resourceSpawnRate, nameof(resourceSpawnRate));
            IsNullCheck(showTrails, nameof(showTrails));
            IsNullCheck(simulationSpeed, nameof(simulationSpeed));
            IsNullCheck(toNormalSimulationSpeedButton, nameof(toNormalSimulationSpeedButton));
        }
        protected override void OnDestroy()
        {
            dronsCount?.onValueChanged.RemoveAllListeners();
            dronsSpeed?.onValueChanged.RemoveAllListeners();
            resourceSpawnRate?.onValueChanged.RemoveAllListeners();
            showTrails?.onValueChanged.RemoveAllListeners();
            simulationSpeed?.onValueChanged.RemoveAllListeners();
            toNormalSimulationSpeedButton?.onClick.RemoveAllListeners();
            base.OnDestroy();
        }
    }
}