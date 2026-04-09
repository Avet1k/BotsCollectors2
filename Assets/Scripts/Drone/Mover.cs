using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 40f;

    public event UnityAction TargetReached;

    public void MoveTo(Vector3 target)
    {
        StartCoroutine(Moving(target));
    }
    
    private IEnumerator Moving(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                _speed * Time.deltaTime);

            yield return null;
        }
        
        TargetReached?.Invoke();
    }
}
