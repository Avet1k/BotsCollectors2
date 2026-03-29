using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _maxCapacity = 10;
    [SerializeField] private int _maxCrystals = 30;
    [SerializeField] private float _spawnInterval = 1f;

    private ObjectPool<Crystal> _pool;
    private int _totalCrystals;

    private void Awake()
    {
        _pool = new ObjectPool<Crystal>(
            createFunc: () => Instantiate(_crystalPrefab),
            actionOnGet: (crystal) => Spawn(crystal),
            actionOnRelease: (crystal) => crystal.SetActive(false),
            actionOnDestroy: (crystal) => Destroy(crystal),
            defaultCapacity: _poolCapacity,
            maxSize: _maxCapacity
        );
    }

    private void Start()
    {
        StartCoroutine(SpawningCrystals());
    }
    
    private IEnumerator SpawningCrystals()
    {
        var wait = new WaitForSeconds(_spawnInterval);
        bool isWorking = true;
        
        while (isWorking)
        {
            if (_totalCrystals < _maxCrystals)
            {
                Crystal crystal = GetCrystal();
                crystal.transform.position = GetRandomPosition();
            }
            
            yield return wait;
        }
    }
    
    public void Spawn(Crystal crystal)
    {
        crystal.SetActive(true);
        _totalCrystals++;
    }

    private Crystal GetCrystal()
    {
        return _pool.Get();
    }

    private void ReleaseCrystal(Crystal crystal)
    {
        _pool.Release(crystal);
        _totalCrystals--;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position;
        Vector3 offsetPosition;
        float offsetY = 20;
        RaycastHit hitinfo;

        do
        {
            float x = Random.Range(_terrain.terrainData.bounds.min.x, _terrain.terrainData.bounds.max.x);
            float y = _terrain.terrainData.bounds.min.y;
            float z = Random.Range(_terrain.terrainData.bounds.min.z, _terrain.terrainData.bounds.max.z);

            position = new Vector3(x, y, z);
            offsetPosition = position + Vector3.up * offsetY;

            Physics.Raycast(offsetPosition, Vector3.down, out hitinfo);
        
        } while (hitinfo.transform.TryGetComponent(out Terrain _) == false);
        
        return position;
    }
}
