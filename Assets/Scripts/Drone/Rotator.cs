using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 60;

    public event UnityAction Rotated;

    public void RotateTowards(Vector3 target)
    {
        StartCoroutine(Rotating(target));
    }

    private IEnumerator Rotating(Vector3 target)
    {
        while (transform.rotation != Quaternion.LookRotation(target - transform.position))
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(target - transform.position),
                _speed * Time.deltaTime);
            
            yield return null;
        }

        Rotated?.Invoke();
    }
}
