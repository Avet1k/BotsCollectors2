using UnityEngine;

public class Graber : MonoBehaviour
{
    [SerializeField] private Vector3 _crystalOffset = new (0.17f, 4.8f, -3.82f);
    
    public bool TryGrab(Crystal crystal)
    {
        bool isGrabbed;

        if (crystal is not null && crystal.isActiveAndEnabled && crystal.transform.position == transform.position)
        {
            crystal.transform.parent = transform;
            crystal.transform.localPosition = _crystalOffset;
            crystal.transform.localRotation = Quaternion.identity;

            isGrabbed = true;
        }
        else
        {
            isGrabbed = false;
        }
        
        return isGrabbed;
    }
}
