using System.Collections;
using UnityEngine;

public class TeethSpawner : NavMeshSpawner<Tooth>
{    
    [Header("Items Spawner")]
    public int _maxItemsCount = 100;
    public int _itemsSpawnBatch = 10;
    public float _spawnRatio = 0.1f;
    public float nextBatchRatio = 2f;
    private int _totalActiveItems = 0;

    public AudioSource aSource;
    public AudioClip spawnSfx;


    private IEnumerator Start()
    {

        while (true)
        {
            if (_totalActiveItems < _maxItemsCount)
            {
                for (int i = 0; i < _itemsSpawnBatch; i++)
                {
                    Tooth itemSpawned = SpawnRandom();

                    if (itemSpawned != null)
                    {
                        aSource?.PlayOneShot(spawnSfx);
                        itemSpawned.Spawner = this;
                        yield return new WaitForSeconds(_spawnRatio);
                        itemSpawned.gameObject.SetActive(true);
                        _totalActiveItems++;
                    }
                }

                yield return new WaitForSeconds(nextBatchRatio);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void ItemDisabled()
    {
        _totalActiveItems--;
    }
}
