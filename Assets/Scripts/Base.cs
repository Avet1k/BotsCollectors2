using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalDetector), typeof(DronesPark), typeof(CrystalCounter))]
public class Base : MonoBehaviour
{
    private CrystalDetector _crystalDetector;
    private DronesPark _dronesPark;
    private CrystalCounter _crystalCounter;
    private List<Crystal> _crystalsInTask;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
        _dronesPark = GetComponent<DronesPark>();
        _crystalsInTask = new List<Crystal>();
        _crystalCounter = GetComponent<CrystalCounter>();
    }

    private void OnEnable()
    {
        _crystalDetector.Detected += OnCrystalDetected;
    }

    private void OnDisable()
    {
        _crystalDetector.Detected -= OnCrystalDetected;
    }

    private void OnCrystalDetected(Crystal crystal)
    {
        if (_crystalsInTask.Contains(crystal))
            return;
        
        if (_dronesPark.TryGetDrone(out Drone drone))
        {
            drone.CrystalIsBrought += TakeCrystal;
            drone.BringCrystal(crystal);
            _crystalsInTask.Add(crystal);
        }
    }

    private void TakeCrystal(Crystal crystal, Drone _)
    {
        if (crystal is null)
            return;
        
        _crystalsInTask.Remove(crystal);
        crystal.Collect();
        _crystalCounter.AddCrystal();
    }
}