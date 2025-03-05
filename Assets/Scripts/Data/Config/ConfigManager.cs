using Utilities;

namespace Data.Config
{
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        private const string ConfigSharePath = "Configs/";

        #region GAME_CONFIG

        public ProductData productData;

        #endregion

        //======================================================

        public void LoadAllConfigLocal()
        {
            if (isLoadedConfigLocal)
                return;

            productData = new ProductData();
            productData.LoadFromAssetPath(ConfigSharePath + "Product");
            
            isLoadedConfigLocal = true;
        }

        private static bool isLoadedConfigLocal = false;
        public static bool IsLoadedConfigLocal
        {
            set { isLoadedConfigLocal = value; }
            get { return isLoadedConfigLocal; }
        }

    }
}