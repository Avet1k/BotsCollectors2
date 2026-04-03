using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private int _dronsCount = 3;

    private Queue<Drone> drones;
    
    private void Start()
    {
        SpawnDrones();
    }

    private void SpawnDrones()
    {
        float offsetX = 20;
        float offsetZ = 30;
        float firstPositionX = transform.position.x - offsetX * (_dronsCount - 1) / 2;
        float positionZ = offsetZ + transform.position.z;
        
        drones = new Queue<Drone>();
        
        for (int i = 0; i < _dronsCount; i++)
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
            
            drones.Enqueue(drone);
        }
    }
}
