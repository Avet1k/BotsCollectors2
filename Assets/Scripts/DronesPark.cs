using System.Collections.Generic;
using UnityEngine;

public class DronesPark : MonoBehaviour
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private int _dronesCount = 3;

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
        float offsetZ = 30;
        float firstPositionX = transform.position.x - offsetX * (_dronesCount - 1) / 2;
        float positionZ = offsetZ + transform.position.z;

        _drones = new Queue<Drone>();

        for (int i = 0; i < _dronesCount; i++)
        {
            Vector3 position = new Vector3(
                firstPositionX + offsetX * i,
                transform.position.y,
                positionZ);

            Drone drone = Instantiate(
                _dronePrefab,
                position,
                Quaternion.identity,
                transform);

            _drones.Enqueue(drone);
        }
    }
}