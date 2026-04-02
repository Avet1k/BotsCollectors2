using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalDetector : MonoBehaviour
{
    [SerializeField] float _detectRadius = 20;
    [SerializeField] float _detectInterval = 1;
    [SerializeField] LayerMask _crystalLayer;
    
    public event UnityAction<Crystal> Detected;
    
    private void Start()
    {
        StartCoroutine(DetectingCrystals());
    }

    private IEnumerator DetectingCrystals()
    {
        var wait = new WaitForSeconds(_detectInterval);
        
        while (true)
        {
            DetectCrystals();
            
            yield return wait;
        }
    }

    private void DetectCrystals()
    {
        Collider[] crystals = Physics.OverlapSphere(transform.position, _detectRadius, _crystalLayer);
        
        foreach (var crystal in crystals)
        {
            Detected?.Invoke(crystal.GetComponent<Crystal>());
        }
    }
}
