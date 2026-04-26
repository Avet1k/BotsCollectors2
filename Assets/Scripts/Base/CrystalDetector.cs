using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CrystalDetector : MonoBehaviour
{
    [SerializeField] float _detectRadius = 20;
    [SerializeField] float _detectInterval = 1;
    [SerializeField] LayerMask _crystalLayer;
    
    public event UnityAction<Crystal> Detected;

    public void StartDetection()
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
        
        foreach (var crystalCollider in crystals)
        {
            Crystal crystal = crystalCollider.GetComponent<Crystal>();

            Detected?.Invoke(crystal);
        }
    }
}
