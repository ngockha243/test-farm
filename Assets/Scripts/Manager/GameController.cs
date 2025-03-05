using System;
using Data.Config;
using Farm;
using UI.Bag;
using UI.SeedBag;
using UI.Shop;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameController : MonoSingleton<GameController>
    {
        private const int workerId = 4;
        [SerializeField] private RectTransform mainCanvas;
        [SerializeField] private GameObject worldButton;

        private FarmDataManager farmDataManager;

        // protected override void Awake()
        // {
        //     farmDataManager = new FarmDataManager();
        // }
        private void Start()
        {
            farmDataManager = new FarmDataManager();
            ConfigManager.Instance.LoadAllConfigLocal();
            Shop.Instance.InitShop();
            GameView.Instance.ShowShop(false);
            Bag.Instance.Init(farmDataManager);
            GameView.Instance.ShowBag(false);
            SeedBag.Instance.Init(farmDataManager);
            GameView.Instance.ShowSeed(false);
        }
        
        private void LoadData()
        {
            int totalProcess = 0;
            // Calculate number of process
            for (int i = 0; i < farmDataManager.Data.workerDatas.Count; i++)
            {
                WorkerData workerData = farmDataManager.Data.workerDatas[i];
                double timeOfProcess = (DateTime.UtcNow - workerData.lastCellAccessTime).TotalSeconds;
                int numberOfProcess = (int)(timeOfProcess / ConfigManager.Instance.productData.GetProductResource(workerId).TimeProcess);
                totalProcess += numberOfProcess;
            }

            for (int i = 0; i < farmDataManager.Data.cellDatas.Count; i++)
            {
                
            }
        }

        public WorldPositionButton SpawnButtonFarm(Cell cell)
        {
            GameObject btn = SimplePool.Spawn(worldButton, Vector3.zero, Quaternion.identity);
            Transform buttonTransform = btn.transform;
            buttonTransform.SetParent(mainCanvas);
            var worldBtn = btn.GetComponent<WorldPositionButton>();
            worldBtn.Initialized(cell.transform, cell.Harvest);
            return worldBtn;
        }

        #region Save Data

        public void UpdateCell(int cellId, int productId, int currentHarvestCount, int hoardedCount,
            DateTime lastHarvestTime)
        {
            farmDataManager.UpdateCell(cellId, productId, currentHarvestCount, hoardedCount, lastHarvestTime);
        }

        public void UpdateBag(int productId, int quantity)
        {
            farmDataManager.UpdateBag(productId, quantity);
        }

        public void UpdateWorker(int workerId, int cellAccess, DateTime lastAccess)
        {
            farmDataManager.UpdateWorkerData(workerId, cellAccess, lastAccess);
        }

        public void PlantProduct(int id)
        {
            int quantity = farmDataManager.Data.GetSeedQuantity(id);
            if (quantity > 0)
            {
                // Plant
                Cell cell = FarmController.Instance.FindCellPlant();
                if (cell != null)
                {
                    cell.PutIn(GameModel.Instance.GetPrefabById(id), ConfigManager.Instance.productData.GetProductResource(id), DateTime.UtcNow, 0, 0);
                }
            }
        }
        public void SellProduct(int productId)
        {
            int quantity = farmDataManager.Data.GetBagQuantity(productId);
            if (quantity > 0)
            {
                farmDataManager.UpdateBag(productId, -quantity);
                int money = quantity * ConfigManager.Instance.productData.GetProductResource(productId).ValuesPerUnit;
                UpdateGold(money);
            }
        }

        public void UpdateGold(int gold)
        {
            farmDataManager.UpdateGold(gold);
            GameView.Instance.ShowGold(farmDataManager.Data.gold);
        }

        public void UpdateLevel()
        {
            farmDataManager.UpdateLevel();
            GameView.Instance.ShowLevel(farmDataManager.Data.farmLevel);
        }

        public void UpdateSeed(int id, int quantity)
        {
            farmDataManager.UpdateSeed(id, quantity);
            
        }

        public void BuyProduct(int productId)
        {
            int price = ConfigManager.Instance.productData.GetProductResource(productId).Price;
            if (farmDataManager.Data.gold < price)
            {
                Debug.Log("Not enough gold");
            }
            else
            {
                int quantity = ConfigManager.Instance.productData.GetProductResource(productId).NumberPerPurchase;
                UpdateGold(-price);
                UpdateSeed(productId, quantity);
            }
        }

        #endregion

        private void OnApplicationQuit()
        {
            farmDataManager.UpdateTimeLastPlayed();
        }
        
    }
}