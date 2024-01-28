using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldToothManager : NavMeshSpawner<Tooth>
{
    public bool thereIsGoldTooth;

    public void TrySpawnGoldTooth()
    {
        if (!thereIsGoldTooth)
        {
            SpawnRandom();
            thereIsGoldTooth = true;
        }
    }
}
