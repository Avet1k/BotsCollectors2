using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CrystalDetector),
    typeof(DronesPark),
    typeof(FlagPlacer))]
public class Base : MonoBehaviour
{
    [SerializeField] private CrystalCounter _crystalCounter;
    [SerializeField] private CrystalReserver _crystalReserver;
    [SerializeField] private int _buildCost = 5;
    [SerializeField] private int _droneCost = 3;
    [SerializeField] private int _startDrones = 0;
    
    private CrystalDetector _crystalDetector;
    private DronesPark _dronesPark;
    private FlagPlacer _flagPlacer;
    private Flag _flag;
    private bool _isClicked;
    private int _dronesReserved = 1;
    private Coroutine _spawningDrone;

    public event UnityAction Clicked;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
        _dronesPark = GetComponent<DronesPark>();
        _flagPlacer = GetComponent<FlagPlacer>();
    }

    private void OnEnable()
    {
        _crystalDetector.Detected += OnCrystalDetected;
    }

    private void OnDisable()
    {
        _crystalDetector.Detected -= OnCrystalDetected;
    }

    private void Start()
    {
        _flag = null;
        
        Initialize(_crystalReserver);
    }                                                    

    private void OnMouseDown()
    {
        if (_isClicked || _dronesPark.DronesAvailable > _dronesReserved)
        {
            _isClicked = true;
            _flagPlacer.FlagOnPlace += SetFlag;
            Clicked?.Invoke();
        }
    }

    public void Initialize(CrystalReserver crystalReserver)
    {
        _dronesPark.SetCrystalReserver(crystalReserver);
        
        if (_crystalReserver is null)
            _crystalReserver = crystalReserver;
        
        _dronesPark.SpawnDrones(_startDrones);
        _spawningDrone = StartCoroutine(SpawningDrones());
        _crystalDetector.StartDetection();
    }
    
    public void RequestParking(Drone drone)
    {
        _dronesPark.ParkNewDrone(drone);
    }

    private void SetFlag(Flag flag)
    {
        _flagPlacer.FlagOnPlace -= SetFlag;
        StopCoroutine(_spawningDrone);
        _isClicked = false;
        _flag = flag;
        
        CountResources();
    }
    
    private void CountResources()
    {
        if (_crystalCounter.Quantity < _buildCost)
        {
            StartCoroutine(WaitingForEnoughCrystals());
            
            return;
        }

        OnEnoughResources();
    }

    private void OnEnoughResources()
    {
        StartCoroutine(GettingDrone());
    }

    private void OnGettingDrone(Drone drone)
    {
        _crystalCounter.RemoveCrystals(_buildCost);
        drone.BuildNewBase(_flag);
        _dronesPark.ReleaseDrone(drone);
        _flag = null;
        _spawningDrone = StartCoroutine(SpawningDrones());
    }

    private void OnCrystalDetected(Crystal crystal)
    {
        if (_crystalReserver.IsCrystalReserved(crystal))
            return;
        
        if (_dronesPark.TryGetDrone(out Drone drone))
        {
            drone.TaskCompleted += TakeCrystal;
            drone.BringCrystal(crystal);
            _crystalReserver.ReserveCrystal(crystal);
        }
    }

    private void TakeCrystal(Crystal crystal, Drone drone)
    {
        drone.TaskCompleted -= TakeCrystal;
        
        if (drone.IsCrystalOnBoard)
        {
            crystal.Collect();
            _crystalCounter.AddCrystal();
        }
        
        _crystalReserver.CancelReservation(crystal);
    }

    private IEnumerator SpawningDrones()
    {
        int dronesAmount = 1;
        bool isWorking = true;
        
        while (isWorking)
        {
            if (_crystalCounter.Quantity >= _droneCost)
            {
                _crystalCounter.RemoveCrystals(_droneCost);
                _dronesPark.SpawnDrones(dronesAmount);
            }
            
            yield return null;
        }
    }

    private IEnumerator WaitingForEnoughCrystals()
    {
        while (_crystalCounter.Quantity < _buildCost)
        {
            yield return null;
        }

        CountResources();
    }

    private IEnumerator GettingDrone()
    {
        bool droneFound = false;
        
        while (droneFound == false)
        {
            if (_dronesPark.TryGetDrone(out Drone drone))
            {
                droneFound = true;
                OnGettingDrone(drone);
            }
                
            yield return null;
        }
    }
}