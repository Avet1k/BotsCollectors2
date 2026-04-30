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
    private int _dronesReserved = 1;
    private Coroutine _spawningDrone;

    private void Awake()
    {
        _crystalDetector = GetComponent<CrystalDetector>();
        _dronesPark = GetComponent<DronesPark>();
        _flagPlacer = GetComponent<FlagPlacer>();
    }

    private void OnEnable()
    {
        _crystalDetector.Detected += OnCrystalDetected;
        _crystalCounter.OnCrystalsChanged += Produce;
    }

    private void OnDisable()
    {
        _crystalDetector.Detected -= OnCrystalDetected;
        _crystalCounter.OnCrystalsChanged -= Produce;
    }

    private void Start()
    {
        _flag = null;

        Initialize(_crystalReserver);
    }

    public void SetBaseBuildFlag(Vector3 position)
    {
        if (_dronesPark.DronesAvailable > _dronesReserved)
            _flag = _flagPlacer.PlaceFlag(position);
    }

    public void Initialize(CrystalReserver crystalReserver)
    {
        _dronesPark.SetCrystalReserver(crystalReserver);

        if (_crystalReserver is null)
            _crystalReserver = crystalReserver;

        _dronesPark.SpawnDrones(_startDrones);
        _crystalDetector.StartDetection();
    }

    public void RequestParking(Drone drone)
    {
        _dronesPark.ParkNewDrone(drone);
    }

    private void Produce(int amount)
    {
        if (_flag is null && amount >= _droneCost)
            SpawnDrone();
        else if (_flag is not null && amount >= _buildCost)
            StartCoroutine(GettingDrone());
    }

    private void OnGettingDrone(Drone drone)
    {
        _crystalCounter.RemoveCrystals(_buildCost);
        drone.BuildNewBase(_flag);
        _dronesPark.ReleaseDrone(drone);
        _flag = null;
        //TODO event counter -
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

    private void SpawnDrone()
    {
        int dronesAmount = 1;
        
        _crystalCounter.RemoveCrystals(_droneCost);
        _dronesPark.SpawnDrones(dronesAmount);
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
