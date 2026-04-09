using UnityEngine;
using UnityEngine.Events;

public class CrystalCounter : MonoBehaviour
{
    private int _quantity = 0;

    public event UnityAction<int> OnCrystalsChanged;

    public void AddCrystal()
    {
        _quantity++;
        OnCrystalsChanged?.Invoke(_quantity);
        Debug.Log($"Crystals: {_quantity}");
    }
}
