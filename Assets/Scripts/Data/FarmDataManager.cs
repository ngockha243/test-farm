using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class FarmDataManager
{
    private const string FilePath = "FarmData.json";

    public FarmData Data { get; private set; }

    public FarmDataManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            Data = JsonConvert.DeserializeObject<FarmData>(json);
        }
        else
        {
            Data = new FarmData();
        }
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
    public void UpdateGold(int gold)
    {
        Data.gold += gold;
        SaveData();
    }

    public void UpdateLevel()
    {
        Data.farmLevel += 1;
        SaveData();
    }

    public void UpdateTimeLastPlayed()
    {
        Data.lastPlayed = DateTime.UtcNow;
        SaveData();
    }

    public void UpdateCell(int cellId, int productId, int currentHarvestCount, int hoardedCount,
        DateTime lastHarvestTime)
    {
        var cell = Data.cellDatas.FirstOrDefault(x => x.cellId == cellId);
        if (cell == null)
        {
            cell = new CellData();
            Data.cellDatas.Add(cell);
        }
        cell.cellId = cellId;
        cell.productId = productId;
        cell.currentHarvestCount = currentHarvestCount;
        cell.hoardedCount = hoardedCount;
        cell.lastHarvestTime = lastHarvestTime;

        SaveData();
    }

    public void UpdateBag(int productId, int quantity)
    {
        var product = Data.bagDatas.FirstOrDefault(x => x.productId == productId);
        if (product == null)
        {
            product = new();
            Data.bagDatas.Add(product);
        }
        product.productId = productId;
        product.productQuantity += quantity;
        
        SaveData();
    }

    public void UpdateWorkerData(int workerId, int cellAccess, DateTime lastAccess)
    {
        var worker = Data.workerDatas.FirstOrDefault(x => x.workerId == workerId);
        if (worker == null)
        {
            worker = new();
            Data.workerDatas.Add(worker);
        }
        worker.workerId = workerId;
        worker.lastCellAccess = cellAccess;
        worker.lastCellAccessTime = lastAccess;
        SaveData();
    }

    public void UpdateSeed(int id, int quantity)
    {
        var seed = Data.bagDatas.FirstOrDefault(x => x.productId == id);
        if (seed == null)
        {
            seed = new();
            Data.bagDatas.Add(seed);
        }
        seed.productId = id;
        seed.productQuantity += quantity;
        
        SaveData();
    }
}
