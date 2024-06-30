using UnityEngine;

namespace Assets.Scripts.Infrastructure.AssetManagment
{
    public class AssetProvider : IAssetProvider
    {
        //public GameObject Instantiate(ToyStaticData toyStaticData, string path, Vector3 at)
        //{
        //    var prefab = Resources.Load<GameObject>(path);
        //    return Object.Instantiate(prefab, at, Quaternion.identity);
        //}

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        //public GameObject InstantiateToy(string path, Vector3 at)
        //{
        //    var prefab = Resources.Load<GameObject>(path);
        //    return Object.Instantiate(prefab, at, Quaternion.identity);
        //}
    }
}
