using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using View;

namespace UI
{
    /// <summary>
    /// Данные в UI по любому кораблю.
    /// </summary>
    public class BaseUIShipInfo : MonoBehaviourLogger
    {
        [SerializeField] private TextMeshProUGUI knotsCountTmp = null;
        [SerializeField] private TextMeshProUGUI canonsCountTmp = null;
        [SerializeField] private TextMeshProUGUI peopleCountTmp = null;
        [SerializeField] private TextMeshProUGUI shipNameTmp = null;
        [SerializeField] private Image hpBatImage = null;

        protected ShipDataWrapper shipDataWrapper;

        protected virtual void UpdateData()
        {
            peopleCountTmp.text = $"{shipDataWrapper.currentPeople}";
            canonsCountTmp.text = $"{shipDataWrapper.currentCannons}";
            knotsCountTmp.text = $"{3}";//ToDo: заполнять после появления расчёта скорости
            hpBatImage.fillAmount = shipDataWrapper.hpPercent;
        }
        private void OnDamageApplyed()
        {
            UpdateData();
        }
        public virtual void SetShip(ShipView shipView)
        {
            shipDataWrapper = shipView.shipDataWrapper;
            shipDataWrapper.damageApplyed += OnDamageApplyed;
            UpdateData();
        }
        protected override void OnDestroy()
        {
            if (shipDataWrapper != null)
            {
                shipDataWrapper.damageApplyed -= OnDamageApplyed;
            }
            base.OnDestroy();
        }
        public bool IsNullCheck1(object checkableObject, string objectName, bool isError = true)
        {
            if (isError && checkableObject == null)
            {
                Debug.LogError($"{objectName} is null!");
            }

            return checkableObject == null;
        }
        protected virtual void Awake()
        {
            IsNullCheck(knotsCountTmp, nameof(knotsCountTmp));
            IsNullCheck(canonsCountTmp, nameof(canonsCountTmp));
            IsNullCheck(peopleCountTmp, nameof(peopleCountTmp));
            IsNullCheck(shipNameTmp, nameof(shipNameTmp));
            IsNullCheck(hpBatImage, nameof(hpBatImage));
        }
    }
}