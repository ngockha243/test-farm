using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameModel : MonoSingleton<GameModel>
    {
        [SerializeField] private List<ProductPrefab> productPrefabs;

        public GameObject GetPrefabById(int id)
        {
            return productPrefabs.FirstOrDefault(x => x.Id == id)?.Prefab;
        }
    }

    [System.Serializable]
    public class ProductPrefab
    {
        public int Id;
        public GameObject Prefab;
    }
}