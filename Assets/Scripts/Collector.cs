using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private readonly int _dronesCount = 3;

    private Queue<Drone> _drones;
    
    public int DronesAvailable => _drones.Count;

    private void Start()
    {
        SpawnDrones();
    }
    
    public void BringCrystal(Crystal crystal)
    {
        if (_drones.Count == 0) return;
        
        Drone drone = _drones.Dequeue();
        drone.BringCrystal(crystal);
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
