using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 40f;
    
    public IEnumerator MovingTo(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                _speed * Time.deltaTime);

            yield return null;
        }
    }
}
