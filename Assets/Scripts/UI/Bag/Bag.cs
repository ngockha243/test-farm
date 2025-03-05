using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Manager;
using UI.Shop;
using UnityEngine;
using Utilities;

namespace UI.Bag
{
    public class Bag : MonoSingleton<Bag>
    {
        [SerializeField] private GameObject shopItem;
        [SerializeField] private Transform itemParent;

        private List<GameObject> bagItem = new();
        private FarmDataManager farmDataManager;

        private bool isInit = false;
        public void Init(FarmDataManager farmDataManager)
        {
            this.farmDataManager = farmDataManager;
            isInit = true;
        }
        public void OnEnable()
        {
            if(isInit == false) return;
            Dlt();
            var lst = ConfigManager.Instance.productData.Records.Where(x => x.FarmType == FarmType.Product).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                var product = lst[i];
                GameObject item = SimplePool.Spawn(shopItem, Vector3.zero, Quaternion.identity);
                Transform itemTransform = item.transform;
                itemTransform.SetParent(itemParent);
                bagItem.Add(item);
                item.GetComponent<Seed>().Init(product.Id, product.ProductName, farmDataManager.Data.GetBagQuantity(product.Id),
                    product.ValuesPerUnit, GameController.Instance.SellProduct, "Sell");
            }
        }

        private void Dlt()
        {
            foreach (var b in bagItem)
            {
                SimplePool.Despawn(b);
            }
        }
    }
}