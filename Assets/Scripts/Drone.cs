using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone: MonoBehaviour
{
    [SerializeField] private float _speed = 40f;

    private Crystal _crystal;

    public void BringCrystal(Crystal crystal)
    {
        _crystal = crystal;
        
        StartCoroutine(Moving(_crystal.transform.position));
    }

    private IEnumerator Moving(Vector3 target)
    {
        while (transform.rotation != Quaternion.LookRotation(target - transform.position))
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(target - transform.position),
                _speed * Time.deltaTime);
            
            yield return null;
        }

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
