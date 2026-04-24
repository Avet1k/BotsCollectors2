using UnityEngine;
using UnityEngine.Events;

public class CrystalCounter : MonoBehaviour
{
    public int Quantity { get; private set; } = 0;

    public event UnityAction<int> OnCrystalsChanged;

    public void AddCrystal()
    {
        Quantity++;
        OnCrystalsChanged?.Invoke(Quantity);
    }

    public void RemoveCrystals(int value)
    {
        Quantity -= value;
        OnCrystalsChanged?.Invoke(Quantity);
    }
}
