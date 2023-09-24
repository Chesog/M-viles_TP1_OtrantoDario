using UnityEngine;

class Taxi : Enemy, IProduct
{

    public GameObject gameObject { get => _GameObject; }

    public void Update()
    {
        //aplicar Flyweight
        throw new System.NotImplementedException();
    }

    public void Instanciate(GameObject prefab)
    {
        _GameObject = GameObject.Instantiate(prefab, _SpawnPoint, Quaternion.identity);
    }
}