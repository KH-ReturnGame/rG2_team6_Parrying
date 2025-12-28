using UnityEngine;

public class DangerIndicator : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 6f;
    [SerializeField] private float minAlpha = 0.25f;
    [SerializeField] private float maxAlpha = 0.6f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sr == null) return;

        // 알파값 깜빡임 (사인파)
        float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
        Color c = sr.color;
        c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        sr.color = c;
    }
}