using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _flag;

    private void Start()
    {
        _flag = Instantiate(_flagPrefab, Vector3.zero, Quaternion.identity, transform);
        _flag.SetActive(false);
    }

    public Flag PlaceFlag(Vector3 position)
    {
        if (_flag.isActiveAndEnabled == false)
            _flag.SetActive(true);

        _flag.transform.position = position;

        return _flag;
    }
}
