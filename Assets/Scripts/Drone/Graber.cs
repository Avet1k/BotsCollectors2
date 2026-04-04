using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Graber : MonoBehaviour
{
    [SerializeField] private Vector3 _crystalOffset = new Vector3 (0.17f, 4.8f, -3.82f);
    
    public event UnityAction<bool> CrystalGrabbed;
    
    public void Grab(Crystal crystal)
    {
        bool isGrabbed;

        if (crystal.transform.position == transform.position)
        {
            crystal.transform.parent = transform;
            crystal.transform.localPosition = _crystalOffset;
            crystal.transform.localRotation = Quaternion.identity;

            isGrabbed = true;
        }
        else
        {
            isGrabbed = false;
        }
        
        CrystalGrabbed?.Invoke(isGrabbed);
    }
}
