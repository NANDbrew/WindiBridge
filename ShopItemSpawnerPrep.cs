using UnityEngine;

namespace WindiBridge
{
    public class ShopItemSpawnerPrep : MonoBehaviour
    {
        public int itemIndex;
        public void Awake()
        {
            GetComponent<ShopItemSpawner>().itemPrefab = PrefabsDirectory.instance.directory[itemIndex];
        }
    }
}
