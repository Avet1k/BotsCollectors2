using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DronesPark : MonoBehaviour
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private int _dronesToSpawn;
    [SerializeField] private SpawnPoint _spawnPoint;

    private Queue<Drone> _drones;
    
    public int DronesAvailable { get; private set; }

    private void Awake()
    {
        _drones = new Queue<Drone>();
        SpawnDrones(_dronesToSpawn);
    }

    public bool TryGetDrone(out Drone drone)
    {
        if (_drones.Count == 0)
        {
            drone = null;
            
            return false;
        }

        drone = _drones.Dequeue();
        drone.TaskCompleted += ParkDrone;
        
        return true;
    }

    public void ParkNewDrone(Drone drone)
    {
        _drones.Enqueue(drone);
        drone.SetReleasePoint(_spawnPoint.transform.position);
        DronesAvailable++;
    }

    public void ReleaseDrone(Drone drone)
    {
        drone.TaskCompleted -= ParkDrone;
        DronesAvailable--;
    }
    
    public void SpawnDrones(int amount)
    {
        float offsetX = 20;
        float firstPositionX = _spawnPoint.transform.position.x - offsetX * (amount - 1) / 2;
        
        for (int i = 0; i < amount; i++)
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
            
            ParkNewDrone(drone);
        }
    }

    private void ParkDrone(Crystal _, Drone drone)
    {
        drone.TaskCompleted -= ParkDrone;
        _drones.Enqueue(drone);
    }
}