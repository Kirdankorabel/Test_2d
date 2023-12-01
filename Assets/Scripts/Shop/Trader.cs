using UnityEngine;

namespace Trade
{
    public class Trader : InteractingObject
    {
        [SerializeField] private GameObject _tradePanel;
        [SerializeField] private TraderInventory _inventory;

        private void Start()
        {
            _tradePanel.gameObject.SetActive(false);
        }

        public override void Use(GameObject user)
        {
            _tradePanel.gameObject.SetActive(true);
            Player.Instance.PlayerInventory.Show(true);
        }
    }
}
