using Services;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class GameWindow : MonoBehaviourLogger
    {
        [SerializeField] private SettingsPanel settingsPanel = null;
        [SerializeField] private Button exitButton = null;
        [SerializeField] private FractionResourcesItem[] fractionResourcesItems = new FractionResourcesItem[0];

        private ServicesContainer servicesContainer;
        public void Initialize(ServicesContainer container)
        {
            servicesContainer = container;

            settingsPanel.Initialize(container.settingsDataService);

            foreach (FractionResourcesItem item in fractionResourcesItems)
            {
                if (!IsNullCheck(item, nameof(fractionResourcesItems)))
                    item.Initialize(servicesContainer.fractionsDataService);
            }
        }
        private void Awake()
        {
            SetLogPrefix(nameof(GameWindow));

            IsNullCheck(settingsPanel, nameof(settingsPanel));
            IsNullCheck(exitButton, nameof(exitButton));

            exitButton.onClick.AddListener(() => Application.Quit());
        }
        protected override void OnDestroy()
        {
            exitButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }
    }
}