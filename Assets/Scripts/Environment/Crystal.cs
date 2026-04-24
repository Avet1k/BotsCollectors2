using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour
{
    public bool IsReserved { get; private set; } = false;
    
    public event UnityAction<Crystal> Collected;
    
    public void Collect()
    {
        Collected?.Invoke(this);
    }

    public void ReserveCrystal()
    {
        IsReserved = true;
    }
    
    public void SetActive(bool value)
    {
        IsReserved = false;
        gameObject.SetActive(value);
    }
}
