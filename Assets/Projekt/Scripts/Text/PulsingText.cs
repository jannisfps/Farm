using UnityEngine;
using TMPro;

public class PulsingText : MonoBehaviour
{
    [Header("Einstellungen")]
    [SerializeField] private float pulseSpeed = 3f; 
    [SerializeField] private float minAlpha = 0.2f;  
    [SerializeField] private float maxAlpha = 1.0f;  

    private TMP_Text _text;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (_text == null) return;

       
        float t = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) / 2f;

        
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        
        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
    }
}