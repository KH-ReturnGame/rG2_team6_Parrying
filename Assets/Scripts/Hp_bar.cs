using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider frontSlider; // 즉시 반영
    [SerializeField] private Slider backSlider;  // 지연 반영

    [Header("Fill Images (optional)")]
    [SerializeField] private Image frontFill;
    [SerializeField] private Image backFill;

    [Header("Color by HP")]
    [SerializeField] private Color highColor = Color.green;
    [SerializeField] private Color midColor  = new Color(1f, 0.85f, 0f); // 노랑
    [SerializeField] private Color lowColor  = Color.red;
    
    [Header("Blink")]
    [SerializeField] private bool enableBlink = true;   // ✅ 추가: 깜빡임 on/off
    [SerializeField, Range(0f, 1f)] private float blinkStartRatio = 0.3f;
    [SerializeField] private float blinkSpeed = 6f;
    [SerializeField] private float blinkMinAlpha = 0.35f;


    [Header("Tuning")]
    [SerializeField] private float backDelay = 0.15f;
    [SerializeField] private float backSmoothTime = 0.25f;
    
    [Header("Success FX (optional)")]
    [SerializeField] private bool enableSuccessFlash = false;
    [SerializeField] private Color successColor = new Color(0.3f, 1f, 1f); // 청록 느낌
    [SerializeField] private float successFlashTime = 0.25f;

    private float successFlashTimer = 0f;
    private Color lastBaseColor = Color.white;

    public void PlaySuccessFlash()
    {
        if (!enableSuccessFlash) return;
        successFlashTimer = successFlashTime;
    }
    
    [Header("Ready FX (for Parry bar)")]
    [SerializeField] private bool enableReadyFx = false;
    [SerializeField] private Color readyColor = new Color(0.2f, 1f, 0.6f); // 초록 네온 느낌
    [SerializeField] private float readyFxTime = 0.25f;

    private float readyFxTimer = 0f;

    public void PlayReadyFx()
    {
        if (!enableReadyFx) return;
        readyFxTimer = readyFxTime;
    }


    
    private float maxHP = 1f;
    private float targetHP;
    private float backVelocity;
    private float delayTimer;
    
    public void Init(float maxHP)
    {
        this.maxHP = Mathf.Max(1f, maxHP);

        if (frontSlider == null || backSlider == null) return;

        frontSlider.minValue = 0;
        backSlider.minValue = 0;

        frontSlider.maxValue = this.maxHP;
        backSlider.maxValue = this.maxHP;

        frontSlider.value = this.maxHP;
        backSlider.value = this.maxHP;

        targetHP = this.maxHP;
        backVelocity = 0f;
        delayTimer = 0f;

        UpdateColorAndBlink(); // 초기 색 적용
    }

    public void SetHP(float hp)
    {
        if (frontSlider == null || backSlider == null) return;

        hp = Mathf.Clamp(hp, 0f, maxHP);

        // 앞바 즉시
        frontSlider.value = hp;

        // 뒤바는 줄어들 때만 딜레이+스무딩
        if (hp < targetHP)
        {
            delayTimer = backDelay;
        }
        else if (hp > targetHP)
        {
            backSlider.value = hp; // 회복은 즉시
            backVelocity = 0f;
            delayTimer = 0f;
        }

        targetHP = hp;

        UpdateColorAndBlink();
    }

    private void Update()
    {
        if (frontSlider == null || backSlider == null) return;

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
        }
        else
        {
            backSlider.value = Mathf.SmoothDamp(backSlider.value, targetHP, ref backVelocity, backSmoothTime);
        }

        // 깜빡임은 프레임마다 반영해야 자연스러움
        UpdateColorAndBlink();
    }

    private void UpdateColorAndBlink()
    {
        if (frontFill == null && backFill == null) return;

        float ratio = (maxHP <= 0f) ? 0f : (targetHP / maxHP);

        // 색: high->mid->low (2구간 보간)
        Color baseColor;
        if (ratio >= 0.5f)
        {
            float t = Mathf.InverseLerp(0.5f, 1f, ratio);
            baseColor = Color.Lerp(midColor, highColor, t);
        }
        else
        {
            float t = Mathf.InverseLerp(0f, 0.5f, ratio);
            baseColor = Color.Lerp(lowColor, midColor, t);
        }

        // 깜빡임(알파 펄스): ratio <= blinkStartRatio
        float alpha = 1f;
        if (enableBlink && ratio <= blinkStartRatio)
        {
            float pulse = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
            alpha = Mathf.Lerp(blinkMinAlpha, 1f, pulse);
        }
        baseColor.a = alpha;


        if (frontFill != null) frontFill.color = baseColor;
        if (backFill != null)  backFill.color  = baseColor;
        lastBaseColor = baseColor;

        if (enableSuccessFlash && successFlashTimer > 0f)
        {
            successFlashTimer -= Time.deltaTime;
            float t = successFlashTimer / successFlashTime; // 1->0
            // 성공색 -> 원래색으로 부드럽게 복귀
            baseColor = Color.Lerp(lastBaseColor, successColor, Mathf.Clamp01(t));
            
            if (enableReadyFx && readyFxTimer > 0f)
            {
                readyFxTimer -= Time.deltaTime;
                float T = readyFxTimer / readyFxTime; // 1 -> 0
                // readyColor로 잠깐 번쩍였다가 원래색으로 복귀
                baseColor = Color.Lerp(baseColor, readyColor, Mathf.Clamp01(T));
            }

        }
    }
}
