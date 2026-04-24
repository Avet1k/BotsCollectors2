using System;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Mover), typeof(Rotator), typeof(Graber))]
[RequireComponent(typeof(Builder))]
public class Drone: MonoBehaviour
{
    private Mover _mover;
    private Rotator _rotator;
    private Graber _grabber;
    private Builder _builder;
    private Crystal _crystal;
    private Vector3 _crystalReleasePoint;
    private Flag _flag;
    
    public bool IsCrystalOnBoard { get; private set; }
    
    public event UnityAction<Crystal, Drone> TaskCompleted;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _rotator = GetComponent<Rotator>();
        _grabber = GetComponent<Graber>();
        _builder = GetComponent<Builder>();
    }

    private void Start()
    {
        _crystal = null;
        _flag = null;
    }

    public void SetReleasePoint(Vector3 point)
    {
        _crystalReleasePoint = point;
    }
    
    public void BringCrystal(Crystal crystal)
    {
        _crystal = crystal;
        _rotator.Rotated += MoveToCrystal;
        _rotator.RotateTowards(_crystal.transform.position);
    }
    
    public void BuildNewBase(Flag flag)
    {
        _flag = flag;
        _rotator.Rotated += MoveToFLag;
        _rotator.RotateTowards(_flag.transform.position);
    }

    private void MoveToFLag()
    {
        _rotator.Rotated -= MoveToFLag;
        _mover.TargetReached += Build;
        _mover.MoveTo(_flag.transform.position);
    }
    
    private void Build()
    {
        _mover.TargetReached -= Build;
        _builder.BaseBuilt += RequestParking;
        _builder.BuildBase();
        _flag.SetActive(false);
        TaskCompleted?.Invoke(null, this);
    }

    private void RequestParking(Base builtBase)
    {
        _builder.BaseBuilt -= RequestParking;
        transform.parent = builtBase.transform;
        builtBase.RequestParking(this);
    }

    private void MoveToCrystal()
    {
        _rotator.Rotated -= MoveToCrystal;
        _mover.TargetReached += GrabCrystal;
        _mover.MoveTo(_crystal.transform.position);
    }

    private void GrabCrystal()
    {
        _mover.TargetReached -= GrabCrystal;
        _grabber.CrystalGrabbed += RotateToBase;
        _grabber.Grab(_crystal);
    }
    
    private void RotateToBase(bool isGrabbed)
    {
        IsCrystalOnBoard = isGrabbed;
        _grabber.CrystalGrabbed -= RotateToBase;
        
        if (IsCrystalOnBoard == false)
        {
            TaskCompleted?.Invoke(_crystal, this);
            _crystal = null;
            
            return;
        }
        
        _rotator.Rotated += MoveToBase;
        _rotator.RotateTowards(_crystalReleasePoint);
    }

    private void MoveToBase()
    {
        _rotator.Rotated -= MoveToBase;
        _mover.TargetReached += ReleaseCrystal;
        _mover.MoveTo(_crystalReleasePoint);
    }

    private void ReleaseCrystal()
    {
        _mover.TargetReached -= ReleaseCrystal;
        TaskCompleted?.Invoke(_crystal, this);

        if (IsCrystalOnBoard)
        {
            _crystal.transform.parent = null;
            _crystal = null;
            IsCrystalOnBoard = false;
        }
    }
}
