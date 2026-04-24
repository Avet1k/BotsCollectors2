using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Base))]
public class FlagPlacer : MonoBehaviour
{
    private const int LeftMouseButton = 0;
    
    [SerializeField] private Flag _flagPrefab;
    
    private Base _base;
    private Flag _flag;
    
    public event UnityAction<Flag> FlagOnPlace;

    private void Awake()
    {
        _base = transform.GetComponent<Base>();
    }

    private void OnEnable()
    {
        _base.Clicked += OnBaseClick;
    }

    private void OnDisable()
    {
        _base.Clicked -= OnBaseClick;
    }

    private void Start()
    {
        _flag = Instantiate(_flagPrefab, Vector3.zero, Quaternion.identity, transform);
        _flag.SetActive(false);
    }

    private void OnBaseClick()
    {
        _base.Clicked -= OnBaseClick;
        StartCoroutine(PlacingFlag());
    }

    private IEnumerator PlacingFlag()
    {
        bool isFlagSet = false;
        float distance = 1000;

        while (isFlagSet == false)
        {
            if (Input.GetMouseButtonDown(LeftMouseButton))
            {
                RaycastHit hit;
                
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, distance);

                if (hit.collider is TerrainCollider)
                {
                    if (_flag.isActiveAndEnabled == false)
                        _flag.SetActive(true);
                    
                    _flag.transform.position = hit.point;
                    isFlagSet = true;
                    _base.Clicked += OnBaseClick;
                    FlagOnPlace?.Invoke(_flag);
                }
            }

            yield return null;
        }
    }
}
