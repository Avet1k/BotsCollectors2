using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CrystalViewer : MonoBehaviour
{
    [SerializeField] private string _header;
    [SerializeField] private CrystalCounter _crystalCounter;
    
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _crystalCounter.OnCrystalsChanged += Change;
    }

    private void OnDisable()
    {
        _crystalCounter.OnCrystalsChanged -= Change;
    }

    private void Change(int value)
    {
        _text.text = _header + value;
    }
}
