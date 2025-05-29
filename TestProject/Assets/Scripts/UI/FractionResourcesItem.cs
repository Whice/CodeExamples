using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class FractionResourcesItem : MonoBehaviourLogger
    {
        [SerializeField] private TextMeshProUGUI textLabel = null;
        [SerializeField] private Image fractionColor = null;
        [SerializeField] private int fractionNumber = 0;

        private FractionsDataService fractionsDataService;

        private void SetCount(int count)
        {
            textLabel.text = $"Fraction resources: {count}";
        }
        private void OnDataChanged()
        {
            SetCount(fractionsDataService.GetResourcesCount(fractionNumber));
        }

        public void Initialize(FractionsDataService service)
        {
            IsNullCheck(service, nameof(FractionsDataService));

            fractionsDataService = service;
            service.dataChanged += OnDataChanged;
            SetCount(0);
        }
        private void Awake()
        {
            SetLogPrefix(nameof(FractionResourcesItem));

            IsNullCheck(textLabel, nameof(textLabel));
            IsNullCheck(fractionColor, nameof(fractionColor));
        }
        protected override void OnDestroy()
        {
            if (fractionsDataService != null)
                fractionsDataService.dataChanged -= OnDataChanged;
            base.OnDestroy();
        }
    }
}