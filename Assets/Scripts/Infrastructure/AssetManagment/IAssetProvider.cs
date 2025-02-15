using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.AssetManagment
{
    public interface IAssetProvider : IService    
    {
        //GameObject Instantiate(ToyStaticData toyStaticData, string path, Vector3 at);
        //GameObject InstantiateToy(string path, Vector3 at);
        GameObject Instantiate(string path);
    }
}
