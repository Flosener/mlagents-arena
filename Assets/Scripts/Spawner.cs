using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.MLAgents;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _collectablePrefab;
    // [SerializeField] private GameObject _agentPrefab;
    [SerializeField] private Transform[] _portalPositions = new Transform[4];
    public static bool spawnEnemies;
    public static int collectablePresent;
    public float spawnIntervalInSeconds;
    private int _portalNr;
    #pragma warning restore 649

    private void Start()
    {
        spawnEnemies = true;
        // Instantiate(_agentPrefab, transform.position, Quaternion.identity);
        StartCoroutine(Spawn());

    }

    private IEnumerator Spawn()
    {
        // Spawn an enemy at random portal position every 'spawnIntervalInSeconds' seconds.
        while (spawnEnemies)
        {
            _portalNr = Random.Range(0,4);
            Transform spawnWall = _portalPositions[_portalNr];
            
            Vector3 position = new Vector3(spawnWall.position.x, 1f, spawnWall.position.z);
            GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            
            // If there are less than three ammo packs, spawn one.
            /*if (collectablePresent < 3)
            {
                GameObject collectable = Instantiate(_collectablePrefab, new Vector3(Random.Range(-6f,6f), 1f, Random.Range(-6f,6f)), Quaternion.identity);
                collectable.transform.SetParent(transform);
                collectablePresent++;
            }*/
            
            yield return new WaitForSeconds(spawnIntervalInSeconds);
        }
    }
    
    // Method to destroy all spawned objects.
    public static void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
