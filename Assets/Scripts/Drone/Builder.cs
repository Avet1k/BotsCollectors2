using UnityEngine;
using UnityEngine.Events;

public class Builder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public event UnityAction<Base> BaseBuilt;
        
    public void BuildBase()
    {
        Base newBase = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        BaseBuilt?.Invoke(newBase);
    }
}
