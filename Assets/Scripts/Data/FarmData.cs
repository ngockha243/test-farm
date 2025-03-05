using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class FarmData
{
    public int gold = 1000;
    public int farmLevel = 1;
    public int workers = 1;
    public DateTime lastPlayed = DateTime.UtcNow;
    public List<WorkerData> workerDatas = new List<WorkerData>();
    public List<CellData> cellDatas = new List<CellData>();
    public List<HoardedData> bagDatas = new List<HoardedData>();
    public List<HoardedData> seedDatas = new List<HoardedData>();

    public int GetBagQuantity(int id)
    {
        var b = bagDatas.FirstOrDefault(x => x.productId == id);
        return b?.productQuantity ?? 0;
    }

    public int GetSeedQuantity(int id)
    {
        var s = seedDatas.FirstOrDefault(x => x.productId == id);
        return s?.productQuantity ?? 0;
    }
}

[Serializable]
public class CellData
{
    public int cellId;
    public int productId;
    public int currentHarvestCount = 0;
    public int hoardedCount = 0;
    public DateTime lastHarvestTime = DateTime.UtcNow;
}

[Serializable]
public class HoardedData
{
    public int productId;
    public int productQuantity = 0;
}

[Serializable]
public class WorkerData
{
    public int workerId;
    public int lastCellAccess;
    public DateTime lastCellAccessTime = DateTime.UtcNow;
}