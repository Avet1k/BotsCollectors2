using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Mover), typeof(Graber))]
public class Drone: MonoBehaviour
{
    [SerializeField] private Vector3 _crystalOffset = new Vector3 (0.17f, 4.8f, -3.82f);
    [SerializeField] private Vector3 _baseOffset = new Vector3(0, 0, 10);
    
    private Mover _mover;
    private Graber _graber;
    private Crystal _crystal;
    
    public event UnityAction<Crystal, Drone> CrystalIsBrought;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _graber = GetComponent<Graber>();
    }

    public void BringCrystal(Crystal crystal)
    {
        _crystal = crystal;
        
        _mover.TargetReached += GrabCrystal;
        _mover.MoveTo(_crystal.transform.position);
    }

    private void GrabCrystal()
    {
        _mover.TargetReached -= GrabCrystal;
        _graber.CrystalGrabbed += MoveToBase;
        _graber.Grab(_crystal);
    }

    private void MoveToBase(bool isGrabbed)
    {
        _graber.CrystalGrabbed -= MoveToBase;
        _mover.TargetReached += GiveCrystal;
        _mover.MoveTo(transform.parent.position + _baseOffset);
        
        if (isGrabbed != true)
            _crystal = null;
    }

    private void GiveCrystal()
    {
        _mover.TargetReached -= GiveCrystal;
        CrystalIsBrought?.Invoke(_crystal, this);

        if (_crystal != null)
        {
            _crystal.transform.parent = null;
            _crystal.Collect();
            _crystal = null;
        }
    }
}
