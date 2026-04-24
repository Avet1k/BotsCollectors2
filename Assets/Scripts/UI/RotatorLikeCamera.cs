using System;
using UnityEngine;

public class RotatorLikeCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
