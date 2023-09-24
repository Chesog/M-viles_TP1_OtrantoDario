
// The Product interface declares the operations that all concrete products
// must implement.
using UnityEngine;

//factory
abstract class Creator
{
    public abstract IProduct FactoryMethod();
}

class TaxiCreator : Creator
{
    public override IProduct FactoryMethod() => new Taxi();
}

class ConoCreator : Creator
{
    public override IProduct FactoryMethod() => new Cono();
}

class BoxCreator : Creator
{
    public override IProduct FactoryMethod() => new Box();
}
public interface IProduct
{
    void Update();

    GameObject gameObject { get; }

    void Instanciate(GameObject prefab);
}

abstract class Enemy
{
    protected GameObject _GameObject;
    protected Vector3 _SpawnPoint;
}

