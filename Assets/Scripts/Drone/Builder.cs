using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Builder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    
    private CrystalReserver _crystalReserver;

    public event UnityAction<Base> BaseBuilt;

    public void SetCrystalReserver(CrystalReserver crystalReserver)
    {
        _crystalReserver = crystalReserver;
    }
    
    public void BuildBase()
    {
        Base newBase = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        newBase.Initialize(_crystalReserver);
        BaseBuilt?.Invoke(newBase);
    }
}
