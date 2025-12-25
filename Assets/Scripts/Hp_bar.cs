using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider frontSlider; // 즉시 반영
    [SerializeField] private Slider backSlider;  // 지연 반영

    [Header("Tuning")]
    [SerializeField] private float backDelay = 0.15f;   // 맞고 난 뒤 지연 시작 시간
    [SerializeField] private float backSmoothTime = 0.25f; // 지연 바가 따라오는 속도

    private float targetHP;
    private float backVelocity;
    private float delayTimer;

    public void Init(float maxHP)
    {
        if (frontSlider == null || backSlider == null) return;

        frontSlider.minValue = 0;
        backSlider.minValue = 0;

        frontSlider.maxValue = maxHP;
        backSlider.maxValue = maxHP;

        frontSlider.value = maxHP;
        backSlider.value = maxHP;

        targetHP = maxHP;
        backVelocity = 0f;
        delayTimer = 0f;
    }

    public void SetHP(float hp)
    {
        if (frontSlider == null || backSlider == null) return;

        // 앞바는 즉시
        frontSlider.value = hp;

        // 뒤바는 "줄어들 때만" 딜레이+스무딩 (회복이면 즉시)
        if (hp < targetHP)
        {
            delayTimer = backDelay; // 새로 맞으면 딜레이 리셋
        }
        else if (hp > targetHP)
        {
            backSlider.value = hp;  // 회복은 즉시
            backVelocity = 0f;
            delayTimer = 0f;
        }

        targetHP = hp;
    }

    private void Update()
    {
        if (frontSlider == null || backSlider == null) return;

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        // 뒤바가 앞바(=targetHP)까지 천천히 따라감
        backSlider.value = Mathf.SmoothDamp(backSlider.value, targetHP, ref backVelocity, backSmoothTime);
    }
}