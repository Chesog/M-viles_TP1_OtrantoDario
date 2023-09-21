using UnityEngine;

class Cono : Enemy, IProduct
{
    public GameObject gameObject { get => _GameObject; }

    public void Instanciate(GameObject prefab)
    {
        _GameObject = GameObject.Instantiate(prefab, _SpawnPoint, Quaternion.identity);
    }

    public void Update()
    {
        //not needed.
    }
}