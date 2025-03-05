using System;
using System.Linq;
using Data.Config;
using Manager;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI.Shop
{
    public class Shop : MonoSingleton<Shop>
    {
        [SerializeField] private GameObject shopItem;
        [SerializeField] private Transform itemParent;
        public void InitShop()
        {
            for (int i = 0; i < ConfigManager.Instance.productData.Records.Count; i++)
            {
                var product = ConfigManager.Instance.productData.Records[i];
                GameObject item = SimplePool.Spawn(shopItem, Vector3.zero, Quaternion.identity);
                Transform itemTransform = item.transform;
                itemTransform.SetParent(itemParent);
                
                item.GetComponent<Seed>().Init(product.Id, product.ProductName, product.NumberPerPurchase, product.Price, GameController.Instance.BuyProduct, "Buy");
            }
        }
    }
}