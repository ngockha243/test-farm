using System.Linq;
using Data.Config;

namespace Data
{
    public class ProductData : SgConfigDataTable<ProductResource>
    {
        protected override void RebuildIndex()
        {
            RebuildIndexByField<int>("Id");
        }

        public ProductResource GetProductResource(int productId)
        {
            ProductResource pro = Records.FirstOrDefault(x => x.Id == productId);
            return pro;
        }
    }

    public class ProductResource
    {
        public int Id;
        public string ProductName;
        public FarmType FarmType;
        public int TimeProcess;
        public int MaxQuantityToHarvest;
        public int ValuesPerUnit;
        public int TimeDispose;
        public int Price;
        public int NumberPerPurchase;
        public int DefaultValue;
    }
}