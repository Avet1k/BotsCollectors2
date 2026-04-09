using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour
{
    public event UnityAction<Crystal> Collected;
    
    public void Collect()
    {
        Collected?.Invoke(this);
    }
    
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
