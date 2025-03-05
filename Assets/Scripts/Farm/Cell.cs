using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
using UnityEngine;

namespace Farm
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Product product;
        [SerializeField] private List<GameObject> lands;
        [SerializeField] private Transform spawnPosition;

        private bool isOpen = false;
        private bool isWorkingOn = false;
        private Worker.Worker workerWorkOn;
        private int cellId = 0;
        
        public int CellId => cellId;
        public Vector3 Position => spawnPosition.position;
        public void SetWorking(Worker.Worker worker, bool value)
        {
            this.workerWorkOn = worker;
            isWorkingOn = value;
        }
        public bool IsWorkingOn => isWorkingOn;
        public bool HasProductToHarvest => product.CurrentProductResource != null && product.CanHarvest();
        public bool CanAccess => product.CurrentProductResource == null;
        private WorldPositionButton worldBtn;
        public void Complete()
        {
            workerWorkOn?.Complete();
            workerWorkOn = null;
            isWorkingOn = false;
        }

        public void Harvest()
        {
            product?.Harvest();
        }
        public void Initialized(int id, bool isOpen, WorldPositionButton worldBtn)
        {
            this.cellId = id;
            lands[0].SetActive(!isOpen);
            lands[1].SetActive(isOpen);
            this.worldBtn = worldBtn;
            product.OnTimeToHarvest += worldBtn.Show;
            product.Initialized(UpdateCell, GameController.Instance.UpdateBag);
        }

        public void PutIn(GameObject prefab, ProductResource productResource, DateTime lastTimeHarvest, int harvestCount, int hoarded)
        {
            product.PutIn(productResource, lastTimeHarvest, harvestCount, hoarded);
            if (productResource != null)
            {
                GameObject productObject = SimplePool.Spawn(prefab, spawnPosition.position, Quaternion.identity);
                Transform productTransform = productObject.transform;
                productTransform.SetParent(spawnPosition);
            }
        }

        public void UpdateCell(int productId, int currentHarvestCount, int hoardedCount,
            DateTime lastHarvestTime)
        {
            GameController.Instance.UpdateCell(cellId, productId, currentHarvestCount, hoardedCount, lastHarvestTime);
        }
    }
}