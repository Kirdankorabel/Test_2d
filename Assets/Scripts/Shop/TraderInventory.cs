using TMPro;
using UnityEditor;
using UnityEngine;

namespace Trade
{
    public class TraderInventory : ItemCollection
    {
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] protected float _priseMultipler = 0.6f;
        [SerializeField] private int _money;
        [SerializeField] private int _itemsCount = 10;

        public int Money
        {
            get => _money;
            set 
            {
                _money = value;
                _moneyText.text = _money.ToString();
            }
        }

        public event System.Action<int> OnMonetChacged;

        private void Awake()
        {
            GameController.Instance.OnLevelLoaded += () => UpdateInventory(GameController.Instance.LevelInfo.TraderPrice);
        }

        protected override void Start()
        {
            base.Start();
            _moneyText.text = _money.ToString();
        }

        public override bool TryToAddItem(ItemInfo item, int cell = -1)
        {
            if (_money < item.Price * _priseMultipler)
            {
                return false;
            }
            if (base.TryToAddItem(item, cell))
            {
                Money -= (int)(item.Price * _priseMultipler);
                OnMonetChacged?.Invoke((int)(item.Price * _priseMultipler));
                return true;
            }
            return false;
        }

        public override bool TryToRemoveItem(ItemInfo item)
        {
            if (PlayerInventory.Instance.Money < item.Price * _priseMultipler)
            {
                return false;
            }
            if (base.TryToRemoveItem(item))
            {
                OnMonetChacged?.Invoke((int)(-item.Price));
                return true;
            }
            return false;
        }

        public void UpdateInventory(int price)
        {
            _money = 1000;
            int counter = 0;
            Items = new ItemInfo[_items.Length];
            var items = GameController.Instance.ItemsData.GetItemsForPrice(price);
            foreach (var item in items)
            {
                base.TryToAddItem(item);
                counter++;
                if (counter == _itemsCount)
                    break;
            }
        }
    }
}
