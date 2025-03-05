using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Farm;
using Farm.Worker;
using UI.Shop;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class FarmController : MonoSingleton<FarmController>
    {
        
        private const int workerId = 4;
        [Header("MVC")]
        [SerializeField] private GameModel model;
        [Header("Land")]
        [SerializeField] private List<Cell> cells;

        [Header("Transform")]
        [SerializeField] private Transform spawnWorkerPosition;

        public Action<Cell> OnSpawnCellBtn;
        private List<Worker> workers = new List<Worker>();
        private int currentWorkerId = 0;
        private FarmDataManager farmDataManager;
        public void Init(FarmDataManager farmDataManager)
        {
            this.farmDataManager = farmDataManager;
            for (int i = 0; i < cells.Count; i++)
            {
                var worldBtn = GameController.Instance.SpawnButtonFarm(cells[i]);
                cells[i].Initialized(i, false, worldBtn);
            }

            for (int i = 0; i < farmDataManager.Data.workerDatas.Count; i++)
            {
                var worker = SpawnWorker();
                Cell cell = FindCellNeedToWork();
                cell.SetWorking(worker, true);
                worker.MoveTo(cell.Position, () => { DoWorkOnCell(cell); }, cell.CellId);
            }
        }

        public void SpawnWorkerWorking()
        {
            var worker = SpawnWorker();
            Cell cell = FindCellNeedToWork();
            cell.SetWorking(worker, true);
            worker.MoveTo(cell.Position, () => { DoWorkOnCell(cell); }, cell.CellId);
        }


        private Worker SpawnWorker()
        {
            GameObject go = SimplePool.Spawn(model.GetPrefabById(workerId), spawnWorkerPosition.position, Quaternion.identity);
            Worker worker = go.GetComponent<Worker>();
            
            Debug.Log(ConfigManager.Instance.productData.GetProductResource(4));
            worker.Initialized(currentWorkerId, OnWorkerCompletePlant, ConfigManager.Instance.productData.GetProductResource(4));
            workers.Add(worker);
            currentWorkerId += 1;
            return worker;
        }
        
        private void OnWorkerCompletePlant(Worker worker)
        {
            Cell cell = FindCellNeedToWork();
            Debug.Log(cell);
            if (cell != null)
            {
                cell.SetWorking(worker, true);
                worker.MoveTo(cell.Position, () => { DoWorkOnCell(cell); }, cell.CellId);
            }
            else
            {
                worker.Waiting();
            }
        }

        public Cell FindCellPlant()
        {
            List<Cell> list = this.cells.Where(
                x => (x.CanAccess && HasSomethingToPlantOrRaiseInCell())).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        private Cell FindCellNeedToWork()
        {
            List<Cell> list = this.cells.Where(
                x => x.IsWorkingOn == false 
                     && (x.HasProductToHarvest == true
                         || (x.CanAccess && HasSomethingToPlantOrRaiseInCell()))).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        private void DoWorkOnCell(Cell cell)
        {
            Debug.Log("DoWorkOnCell");
            cell.Complete();
            if (cell.CanAccess)
            {
                // TODO: Plant
                PlantOrRaiseInCell(cell);
            }
            else if(cell.HasProductToHarvest)
            {
                // TODO: Harvest
                HarvestCell(cell);
            }
        }

        private void PlantOrRaiseInCell(Cell cell)
        {
            // Test
            var product = ConfigManager.Instance.productData.GetProductResource(2);
            cell.PutIn(model.GetPrefabById(product.Id), product, DateTime.Now, 0, 0);
        }

        private void HarvestCell(Cell cell)
        {
            // Test
            cell.Harvest();
        }

        private bool HasSomethingToPlantOrRaiseInCell()
        {
            return true;
        }

        private void WorkerDistribution()
        {
            Cell cell = FindCellNeedToWork();
            Worker worker = workers.FirstOrDefault(x => x.IsFree == true);
            worker.MoveTo(cell.Position, () =>
            {
                DoWorkOnCell(cell);
            }, cell.CellId);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 20;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Time.timeScale = 1;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Cell cell = hit.collider.GetComponent<Cell>();
                    cell.Complete();
                }
            }
        }
    }
}