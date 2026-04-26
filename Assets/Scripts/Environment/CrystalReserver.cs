using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReserver : MonoBehaviour
{
    private List<Crystal> _reservedCrystals;

    private void Start()
    {
        _reservedCrystals = new List<Crystal>();
    }

    public bool IsCrystalReserved(Crystal crystal) => _reservedCrystals.Contains(crystal);
        
    public void ReserveCrystal(Crystal crystal)
    {
        if (IsCrystalReserved(crystal))
            return;
        
        _reservedCrystals.Add(crystal);
    }

    public void CancelReservation(Crystal crystal)
    {
        if (_reservedCrystals.Contains(crystal))
            _reservedCrystals.Remove(crystal);
    }
}
