using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalDetector), typeof(Collector))]
public class Base : MonoBehaviour
{
    private CrystalDetector _crystalDetector;
    private Collector _collector;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
        _collector = GetComponent<Collector>();
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
        if (_collector.DronesAvailable > 0)
        {
            _collector.BringCrystal(crystal);
        }
    }
}
