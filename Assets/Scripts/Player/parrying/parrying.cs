using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    private bool canParry = true;
    private bool parryWindowActive = false; // 패링 윈도우 활성 상태
    private float parryCooldown = 5f;  // 쿨다운 시간
    private float cooldownTimer = 0f;
    private float parryWindowDuration = 0.25f; // 패링 윈도우 지속 시간 (0.25초)
    private float parryWindowTimer = 0f;
    public PlayerReinforceAttack code;

    void awake()
    {
        // 초기화
        canParry = true;
        parryWindowActive = false;
        cooldownTimer = 0f;
        parryWindowTimer = 0f;
    }
    void Update()
    {
        HandleCooldown();
        HandleParryWindow();

        // 패링 입력
        if (Input.GetKeyDown(KeyCode.LeftShift) && canParry)
        {
            StartParryWindow();
        }
    }

    /// <summary>
    /// 쿨다운 관리
    /// </summary>
    private void HandleCooldown()
    {
        if (!canParry)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canParry = true;
                Debug.Log("Parry ready again!");
            }
        }
    }

    /// <summary>
    /// 패링 윈도우 관리
    /// </summary>
    private void HandleParryWindow()
    {
        if (parryWindowActive)
        {
            parryWindowTimer -= Time.deltaTime;

            // 윈도우 끝나면 자동 실패 처리
            if (parryWindowTimer <= 0f)
            {
                EndParryWindow(false);
            }
        }
    }

    /// <summary>
    /// 패링 시도 시작 -> 짧은 시간만 패링 가능
    /// </summary>
    private void StartParryWindow()
    {
        parryWindowActive = true;
        parryWindowTimer = parryWindowDuration;
        Debug.Log("Parry Window Opened!");
    }

    /// <summary>
    /// 공격이 들어올 때 호출되는 함수
    /// (실제 전투 시스템에서 공격 타이밍 맞춰 호출 필요)
    /// </summary>
    public void OnTriggerEnter2D(Collider2D attack)
    {
        if (parryWindowActive && attack.GetComponent<EnemyAttack>().canBeParried)
        {
            // 성공
            EndParryWindow(true, attack);
        }
        else
        {
            // 실패
            EndParryWindow(false);
        }
    }

    /// <summary>
    /// 패링 윈도우 종료
    /// </summary>
    private void EndParryWindow(bool success, Collider2D attack = null)
    {
        parryWindowActive = false;

        if (success)
        {
            Debug.Log("Parry Success!");
            attack?.GetComponent<EnemyAttack>().CancelAttack();
            code.RegisterParrySuccess();
        }
        else
        {
            Debug.Log("Parry Failed! Cooldown for 5s.");
            canParry = false;
            cooldownTimer = parryCooldown;
        }
    }
}