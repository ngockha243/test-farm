using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Farm
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private TextMeshPro productName;
        [SerializeField] private TextMeshPro productTime;
        [SerializeField] private TextMeshPro productQuantity;
        public ProductResource CurrentProductResource;
        
        private int currentHarvestCount = 0;
        private int hoardedCount = 0;
        private int timeCount = 0;
        private DateTime lastCountHarvestTime;
        private bool isDisposing = false;
        public Action<bool> OnTimeToHarvest;
        private Action<int, int, int, DateTime> OnUpdateCell;
        private Action<int, int> OnUpdateBag;
        private void Start()
        {
            TimerController.Instance.AddAction(SecondChanged);
        }

        public void Initialized(Action<int, int, int, DateTime> onUpdate, Action<int, int> onUpdateBag)
        {
            OnTimeToHarvest?.Invoke(false);
            this.OnUpdateCell = onUpdate;
            this.OnUpdateBag = onUpdateBag;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Harvest();
            }
        }

        public void PutIn(ProductResource productResource, DateTime lastTimeHarvest, int harvestCount = 0, int hoardedCount = 0)
        {
            CurrentProductResource = productResource;
            currentHarvestCount = harvestCount;
            lastCountHarvestTime = lastTimeHarvest;
            hoardedCount = hoardedCount;
            if(productResource != null)
            {
                Calculate();
                productName.text = productResource.ProductName;
            }
            else
            {
                OnTimeToHarvest?.Invoke(false);
            }
            OnUpdateCell?.Invoke(CurrentProductResource.Id, currentHarvestCount, hoardedCount, lastCountHarvestTime);
        }
        private void Calculate()
        {
            double secondFromOfflineToOnline = (DateTime.UtcNow - lastCountHarvestTime).TotalSeconds;
            int numAutoHarvestEstimateWhenOffline = (int)(secondFromOfflineToOnline / CurrentProductResource.TimeProcess);
            int numRemainCanHarvest = CurrentProductResource.MaxQuantityToHarvest - currentHarvestCount;
            int numRealCanHarvest = Mathf.Min(numRemainCanHarvest, numAutoHarvestEstimateWhenOffline);

            hoardedCount = numRealCanHarvest > 0 ? numRealCanHarvest : 0;
            OnTimeToHarvest?.Invoke(hoardedCount > 0);
            if (numRealCanHarvest + currentHarvestCount >= CurrentProductResource.MaxQuantityToHarvest)
            {
                DateTime timeLastProductCanHarvest = lastCountHarvestTime.AddSeconds(numRealCanHarvest * CurrentProductResource.TimeProcess);
                double timeDifFromLastProductHarvest = (DateTime.UtcNow - timeLastProductCanHarvest).TotalSeconds;
                if (timeDifFromLastProductHarvest >= CurrentProductResource.TimeDispose)
                {
                    Dispose();
                }

                timeCount = (int)timeDifFromLastProductHarvest;
            }
            else
            {
                int remainTime = (int)secondFromOfflineToOnline - numRealCanHarvest * CurrentProductResource.TimeProcess;
                timeCount = remainTime > 0 ? remainTime : CurrentProductResource.TimeProcess;
            }

            UpdateQuantityText();
        }
        public bool CanHarvest()
        {
            return currentHarvestCount < CurrentProductResource.MaxQuantityToHarvest 
                && hoardedCount > 0;
        }
        public void Harvest()
        {
            if(CanHarvest() == false) return;
            currentHarvestCount += hoardedCount;
            hoardedCount = 0;
            if (currentHarvestCount >= CurrentProductResource.MaxQuantityToHarvest)
            {
                Dispose();
            }
            UpdateQuantityText();
            OnTimeToHarvest?.Invoke(false);
            
            OnUpdateCell?.Invoke(CurrentProductResource.Id, currentHarvestCount, hoardedCount, lastCountHarvestTime);
            OnUpdateBag?.Invoke(CurrentProductResource.Id, hoardedCount);
        }

        private void UpdateQuantityText()
        {
            if(CurrentProductResource != null)
                productQuantity.text = $"{hoardedCount} - {currentHarvestCount}/{CurrentProductResource.MaxQuantityToHarvest}";
        }

        public void Dispose()
        {
            CurrentProductResource = null;
            currentHarvestCount = 0;
            hoardedCount = 0;
            isDisposing = false;
            
            productTime.text = "";
            productName.text = "";
            productQuantity.text = "";
            OnTimeToHarvest?.Invoke(false);
            
            OnUpdateCell?.Invoke(-1, 0, 0, DateTime.UtcNow);
        }

        public void SecondChanged()
        {
            if(CurrentProductResource == null) return;

            timeCount -= 1;
            productTime.text = timeCount.ToString();
            if (timeCount <= 0)
            {
                if ((currentHarvestCount + hoardedCount) < CurrentProductResource.MaxQuantityToHarvest)
                {
                    hoardedCount += 1;
                    timeCount = CurrentProductResource.TimeProcess;
                    UpdateQuantityText();
                    lastCountHarvestTime = DateTime.UtcNow;
                    OnTimeToHarvest?.Invoke(true);
                    
                    OnUpdateCell?.Invoke(CurrentProductResource.Id, currentHarvestCount, hoardedCount, lastCountHarvestTime);
                }
                else
                {
                    Dispose();
                }
            }
        }
    }
}