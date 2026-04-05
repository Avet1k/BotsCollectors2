using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalDetector), typeof(Collector), typeof(CrystalCounter))]
public class Base : MonoBehaviour
{
    private CrystalDetector _crystalDetector;
    private Collector _collector;
    private CrystalCounter _crystalCounter;
    private List<Crystal> _crystalsInTask;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
        _collector = GetComponent<Collector>();
        _crystalsInTask = new List<Crystal>();
        _crystalCounter = GetComponent<CrystalCounter>();
    }

    private void OnEnable()
    {
        _crystalDetector.Detected += OnCrystalDetected;
        _collector.CrystalIsBrought += TakeCrystal;
    }

    private void OnDisable()
    {
        _crystalDetector.Detected -= OnCrystalDetected;
        _collector.CrystalIsBrought -= TakeCrystal;
    }

    private void OnCrystalDetected(Crystal crystal)
    {
        if (_crystalsInTask.Contains(crystal))
            return;
        
        if (_collector.DronesAvailable > 0)
        {
            _collector.BringCrystal(crystal);
            _crystalsInTask.Add(crystal);
        }
    }

    private void TakeCrystal(Crystal crystal)
    {
        if (crystal is null)
            return;

        _crystalsInTask.Remove(crystal);
        _crystalCounter.AddCrystal();
    }
}