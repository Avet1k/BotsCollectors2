using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Mover), typeof(Rotator), typeof(Graber))]
public class Drone: MonoBehaviour
{
    [SerializeField] private Vector3 _baseOffset = new (0, 0, 10);
    
    private Mover _mover;
    private Rotator _rotator;
    private Graber _grabber;
    private Crystal _crystal;
    
    public event UnityAction<Crystal, Drone> CrystalIsBrought;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _rotator = GetComponent<Rotator>();
        _grabber = GetComponent<Graber>();
    }

    public void BringCrystal(Crystal crystal)
    {
        _crystal = crystal;

        _rotator.Rotated += MoveToCrystal;
        _rotator.RotateTowards(_crystal.transform.position);
    }

    private void MoveToCrystal()
    {
        _mover.TargetReached += GrabCrystal;
        _mover.MoveTo(_crystal.transform.position);
    }

    private void GrabCrystal()
    {
        _mover.TargetReached -= GrabCrystal;
        _grabber.CrystalGrabbed += MoveToBase;
        _grabber.Grab(_crystal);
    }

    private void MoveToBase(bool isGrabbed)
    {
        _grabber.CrystalGrabbed -= MoveToBase;
        _mover.TargetReached += ReleaseCrystal;
        _mover.MoveTo(transform.parent.position + _baseOffset);
        
        if (isGrabbed != true)
            _crystal = null;
    }

    private void ReleaseCrystal()
    {
        _mover.TargetReached -= ReleaseCrystal;
        CrystalIsBrought?.Invoke(_crystal, this);

        if (_crystal is not null)
        {
            _crystal.transform.parent = null;
            _crystal = null;
        }
    }
}
