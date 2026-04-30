using System.Collections;
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
    private CrystalReserver _crystalReserver;
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

    public void SetCrystalReserver(CrystalReserver crystalReserver)
    {
        _crystalReserver = crystalReserver;
        _builder.SetCrystalReserver(_crystalReserver);
    }

    public void SetReleasePoint(Vector3 point)
    {
        _crystalReleasePoint = point;
    }

    public IEnumerator BringingCrystal(Crystal crystal)
    {
        _crystal = crystal;
        
        yield return _rotator.SmoothRotating(_crystal.transform.position);

        yield return _mover.MovingTo(_crystal.transform.position);

        IsCrystalOnBoard = _grabber.TryGrab(_crystal);
        
        if (IsCrystalOnBoard == false)
        {
            TaskCompleted?.Invoke(_crystal, this);
            _crystal = null;

            yield break;
        }
        
        yield return _rotator.SmoothRotating(_crystalReleasePoint);
        
        yield return _mover.MovingTo(_crystalReleasePoint);
        
        TaskCompleted?.Invoke(_crystal, this);

        if (IsCrystalOnBoard)
        {
            _crystal.transform.parent = null;
            _crystal = null;
            IsCrystalOnBoard = false;
        }
    }
    
    public IEnumerator BuildingNewBase(Flag flag)
    {
        _flag = flag;
        
        yield return _rotator.SmoothRotating(flag.transform.position);
        
        yield return _mover.MovingTo(_flag.transform.position);
        
        Base builtBase = _builder.BuildBase();
        _flag.SetActive(false);
        TaskCompleted?.Invoke(null, this);
        
        transform.parent = builtBase.transform;
        builtBase.RequestParking(this);
    }
}
