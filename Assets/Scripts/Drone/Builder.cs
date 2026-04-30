using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    
    private CrystalReserver _crystalReserver;

    public void SetCrystalReserver(CrystalReserver crystalReserver)
    {
        _crystalReserver = crystalReserver;
    }
    
    public Base BuildBase()
    {
        Base newBase = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        newBase.Initialize(_crystalReserver);
        return newBase;
    }
}
