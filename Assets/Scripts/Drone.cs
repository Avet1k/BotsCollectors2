using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone: MonoBehaviour
{
    [SerializeField] private float _speed;

    private Crystal _crystal;

    public void BringCrystal(Crystal crystal)
    {
        _crystal = crystal;
        StartCoroutine(Moving(_crystal.transform.position));
        GrabCrystal(_crystal);
    }

    private IEnumerator Moving(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void GrabCrystal(Crystal crystal)
    {
        // TODO: Pick up the crystal
    }
}
