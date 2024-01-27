using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnRadio
    {
        public float spawnRadio;
        public float spawnProbability;
    }

    [System.Serializable]
    public struct PrefabProbabilityPair<U>
    {
        public U Prefab;
        public float SpawnProbability;
    }


    private const int maxSpawnTries = 20;

    [Header("Spawner Data (less probability to more. From 0 to 1)")]
    public PrefabProbabilityPair<T>[] Prefabs;

    public SpawnRadio[] _spawnRadios;
    public Renderer meshGround;

    internal List<List<T>> _spawnPool = new List<List<T>>();

    protected virtual void OnValidate()
    {
        if (_spawnRadios == null ||_spawnRadios.Length == 0)
        {
            _spawnRadios = new SpawnRadio[1];
            _spawnRadios[0].spawnRadio = 40;
            _spawnRadios[0].spawnProbability = 1f;
        }
    }

    protected virtual void Awake()
    {
        for (int i=0; i < Prefabs.Length; i++)
        {
            _spawnPool.Add(new List<T>());
        }
    }

    public int GetRandomPrefab()
    {
        float dice = Random.value;

        for (int i = 0; i < Prefabs.Length; i++)
        {
            if (dice <= Prefabs[0].SpawnProbability || 
                (dice <= Prefabs[i].SpawnProbability && dice >= Prefabs[i-1].SpawnProbability))
            {
                return i;
            }
        }

        return Prefabs.Length - 1;
    }

    public T SpawnRandom()
    {
        T spawned = GetSpawnable();

        Vector3 spawnPoint;
        bool willSpawn = false;
        int spawnTries = 0;

        do
        {
            spawnPoint = GetRandomPointInPlane(meshGround);

            float distanceToSpawner = (transform.position - spawnPoint).sqrMagnitude;

            for (int x = _spawnRadios.Length - 1; x >= 0 && !willSpawn; x--)
            {
                if (distanceToSpawner < _spawnRadios[x].spawnRadio * _spawnRadios[x].spawnRadio)
                {
                    if (Random.value <= _spawnRadios[x].spawnProbability)
                    {
                        willSpawn = true;
                    }
                }
            }

            spawnTries++;

        } while (!willSpawn && spawnTries < maxSpawnTries);


        if (GetNavMeshPoint(ref spawnPoint))
        {
            spawned.transform.position = spawnPoint;
            return spawned;
        }
        else
        {
            spawned.gameObject.SetActive(false);
        }

        return null;
    }

    public bool GetNavMeshPoint(ref Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            position = hit.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    public T GetSpawnable()
    {
        int randomSpawnable = GetRandomPrefab();
        foreach (T spawnable in _spawnPool[randomSpawnable])
        {
            if (!spawnable.gameObject.activeSelf)
            {
                return spawnable;
            }
        }

        T newSpawnlable = Instantiate(Prefabs[randomSpawnable].Prefab, transform);
        _spawnPool[randomSpawnable].Add(newSpawnlable);

        return newSpawnlable;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (SpawnRadio sp in _spawnRadios)
        {
            Gizmos.color = Color.Lerp(Color.red, Color.blue, sp.spawnProbability);
            CustomGizmos.DrawWireCircle(transform.position, transform.up, sp.spawnRadio);
        }
    }


    public static Vector3 GetRandomPointInPlane(Renderer meshPlane)
    {
        Bounds groundLocalBounds = meshPlane.localBounds;
        Vector3 randomPoint = new Vector3(groundLocalBounds.center.x + Random.Range(-1f, 1f) * groundLocalBounds.extents.x,
                groundLocalBounds.center.y,
                groundLocalBounds.center.x + Random.Range(-1f, 1f) * groundLocalBounds.extents.z);

        return meshPlane.transform.TransformPoint(randomPoint);
    }
}
