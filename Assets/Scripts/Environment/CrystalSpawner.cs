using System.Collections;
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
    [Tooltip("Определяет отступ появления кристалов от края в процентах")]
    [SerializeField, Range(1, 99)] private float _offset = 25;

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

    private void Spawn(Crystal crystal)
    {
        crystal.SetActive(true);
        _totalCrystals++;
    }

    private Crystal GetCrystal()
    {
        Crystal crystal = _pool.Get();
        crystal.Collected += ReleaseCrystal;
        
        return crystal;
    }

    private void ReleaseCrystal(Crystal crystal)
    {
        crystal.Collected -= ReleaseCrystal;
        _pool.Release(crystal);
        _totalCrystals--;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position;
        Vector3 offsetPosition;
        float offsetY = 20;
        RaycastHit hitinfo;
        var bounds = _terrain.terrainData.bounds;
        float percentConverter = 100f;
        float spawnOffsetX = (bounds.max.x - bounds.min.x) / percentConverter * _offset;
        float spawnOffsetZ = (bounds.max.y - bounds.min.y) / percentConverter * _offset;
        
        do
        {
            float x = Random.Range(bounds.min.x + spawnOffsetX, bounds.max.x - spawnOffsetX);
            float y = _terrain.terrainData.bounds.min.y;
            float z = Random.Range(bounds.min.z + spawnOffsetZ, bounds.max.z - spawnOffsetZ);

            position = new Vector3(x, y, z);
            offsetPosition = position + Vector3.up * offsetY;

            Physics.Raycast(offsetPosition, Vector3.down, out hitinfo);

        } while (hitinfo.transform.TryGetComponent(out Terrain _) == false);

        return position;
    }
}
