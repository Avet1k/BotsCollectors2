using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalDetector))]
public class Base : MonoBehaviour
{
    private CrystalDetector _crystalDetector;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
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
        
    }
}
