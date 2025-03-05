using TMPro;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameView : MonoSingleton<GameView>
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI workerText;

        [SerializeField] private GameObject shop;
        [SerializeField] private GameObject bag;
        [SerializeField] private GameObject seed;
        public void ShowGold(int gold)
        {
            goldText.text = $"Gold: {gold}";
        }

        public void ShowLevel(int level)
        {
            levelText.text = $"Level: {level}";
        }

        public void ShowWorker(int working, int free)
        {
            workerText.text = $"Working: {working} - Free: {free}";
        }

        public void ShowShop(bool show)
        {
            shop.SetActive(show);
        }
        public void ShowBag(bool show)
        {
            bag.SetActive(show);
        }

        public void ShowSeed(bool show)
        {
            seed.SetActive(show);
        }
    }
}