using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Shop
{
    public class Seed : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI btnText;

        private Action<int> onPurchase;
        private int id;
        public void Init(int id, string name, int num, int price, Action<int> onBuy, string btn)
        {
            this.id = id;
            nameText.text = name;
            coinText.text = $"{num}/{price} GOLD";
            onPurchase = onBuy;
            btnText.text = btn;
        }
        public void Init(int id, string name, int num, Action<int> onBuy, string btn)
        {
            this.id = id;
            nameText.text = name;
            coinText.text = $"{num}";
            onPurchase = onBuy;
            btnText.text = btn;
        }

        public void Purchase()
        {
            onPurchase?.Invoke(id);
        }
    }
}