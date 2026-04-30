using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    private const int LeftMouseButton = 0;

    private Camera _camera;

    public event Action<RaycastHit> OnHit;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                OnHit?.Invoke(hit);
            }
        }
    }
}
