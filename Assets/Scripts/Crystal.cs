using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour
{
    public event UnityAction<Crystal> Transferred;
    
    public void Transfer()
    {
        Transferred?.Invoke(this);
    }
    
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
