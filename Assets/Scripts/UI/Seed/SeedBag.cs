using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Manager;
using UI.Shop;
using UnityEngine;
using Utilities;

namespace UI.SeedBag
{

    public class SeedBag : MonoSingleton<SeedBag>
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
                item.GetComponent<Seed>().Init(product.Id, product.ProductName, farmDataManager.Data.GetSeedQuantity(product.Id), GameController.Instance.PlantProduct, "Plant");
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