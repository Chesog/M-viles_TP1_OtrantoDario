using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<IProduct> enemies = new List<IProduct>();
    private List<Transform> enemiesSpawnPos;
    public int _boxCuantity = 1;
    public int _conoCuantity = 1;
    public int _taxiCuantity = 1;


    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Update();
        }
    }
    void Start()
    {
        Creator c = null;
        c = new BoxCreator();

        for (int i = 0; i < _boxCuantity; i++)
        {
            enemies.Add(c.FactoryMethod());
        }
        c = new ConoCreator();

        for (int i = 0; i < _conoCuantity; i++)
        {
            enemies.Add(c.FactoryMethod());
        }

        c = new TaxiCreator();

        for (int i = 0; i < _taxiCuantity; i++)
        {
            enemies.Add(c.FactoryMethod());
        }
    }
}
