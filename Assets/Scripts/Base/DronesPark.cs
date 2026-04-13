using System.Collections.Generic;
using UnityEngine;

public class DronesPark : MonoBehaviour
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private int _dronesCount = 3;
    [SerializeField] private SpawnPoint _spawnPoint;

    private Queue<Drone> _drones;

    private void Start()
    {
        SpawnDrones();
    }

    public bool TryGetDrone(out Drone drone)
    {
        if (_drones.Count == 0)
        {
            drone = null;
            
            return false;
        }

        drone = _drones.Dequeue();
        drone.CrystalIsBrought += ReleaseDrone;
        
        return true;
    }

    private void ReleaseDrone(Crystal _, Drone drone)
    {
        drone.CrystalIsBrought -= ReleaseDrone;
        _drones.Enqueue(drone);
    }

    private void SpawnDrones()
    {
        float offsetX = 20;
        float firstPositionX = _spawnPoint.transform.position.x - offsetX * (_dronesCount - 1) / 2;

        _drones = new Queue<Drone>();

        for (int i = 0; i < _dronesCount; i++)
        {
            Vector3 position = new Vector3(
                firstPositionX + offsetX * i,
                _spawnPoint.transform.position.y,
                _spawnPoint.transform.position.z);

            Drone drone = Instantiate(
                _dronePrefab,
                position,
                _spawnPoint.transform.rotation,
                transform);
            
            drone.SetReleasePoint(_spawnPoint.transform.position);
            _drones.Enqueue(drone);
        }
    }
}