using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputService))]
public class RaycastHandler : MonoBehaviour
{
    private InputService _inputService;
    private Base _base;

    private void Awake()
    {
        _inputService = GetComponent<InputService>();
    }

    private void OnEnable()
    {
        _inputService.OnHit += CheckHit;
    }

    private void OnDisable()
    {
        _inputService.OnHit -= CheckHit;
    }

    private void CheckHit(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Base baseComponent))
        {
            _base = baseComponent;
        }

        if (hit.collider is TerrainCollider)
        {
            if (_base == null)
                return;

            _base.SetBaseBuildFlag(hit.point);
            _base = null;
        }
    }
}
