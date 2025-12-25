using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    private bool canParry = true;
    private bool parryWindowActive = false; // 패링 윈도우 활성 상태
    private float parryCooldown = 5f;  // 쿨다운 시간
    private float cooldownTimer = 0f;
    private float parryWindowDuration = 0.75f; // 패링 윈도우 지속 시간 (0.75초)
    private float parryWindowTimer = 0f;

    public PlayerReinforceAttack code;

    //가드(데미지 감소) 설정 
    [Header("Guard (Q)")]
    [SerializeField] private KeyCode guardKey = KeyCode.Q;
    [Range(0f, 1f)]
    [SerializeField] private float guardDamageMultiplier = 0.5f; // 50% 데미지
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        canParry = true;
        parryWindowActive = false; // [수정] false;w -> false;
        cooldownTimer = 0f;
        parryWindowTimer = 0f;
    }

    private void Update()
    {
        HandleCooldown();
        HandleParryWindow();

        // 패링 입력
        if (Input.GetKeyDown(KeyCode.LeftShift) && canParry)
        {
            StartParryWindow();
        }
    }

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

    private void StartParryWindow()
    {
        parryWindowActive = true;
        parryWindowTimer = parryWindowDuration;
        Debug.Log("Parry Window Opened!");
    }

    private void OnTriggerEnter2D(Collider2D attack)
    {
        var enemyAttack = attack.GetComponent<EnemyAttack>();
        if (enemyAttack == null) return;
        if (parryWindowActive)
        {
            if (enemyAttack.canBeParried)
            {
                EndParryWindow(true, attack);
            }
            else
            {
                EndParryWindow(false);
            }
            return;
        }

        if (!canParry && Input.GetKey(guardKey) && IsAttackFromFront(attack.transform.position))
        {
            enemyAttack.ApplyDamageMultiplierOnce(guardDamageMultiplier);
            Debug.Log("Guard! Damage reduced to 50% (front only).");
        }

    }

    private void EndParryWindow(bool success, Collider2D attack = null)
    {
        parryWindowActive = false;

        if (success)
        {
            Debug.Log("Parry Success!");

            var enemyAttack = attack != null ? attack.GetComponent<EnemyAttack>() : null;
            enemyAttack?.CancelAttack();

            if (code != null) code.RegisterParrySuccess();
        }
        else
        {
            Debug.Log("Parry Failed! Cooldown for 5s.");
            canParry = false;
            cooldownTimer = parryCooldown;
        }
    }

    private bool IsAttackFromFront(Vector2 attackerPos)
    {
        float facingSign;

        if (spriteRenderer != null)
            facingSign = spriteRenderer.flipX ? -1f : 1f;
        else
            facingSign = Mathf.Sign(transform.localScale.x);

        if (Mathf.Approximately(facingSign, 0f)) facingSign = 1f;

        Vector2 facingDir = new Vector2(facingSign, 0f); // 2D에서 좌/우만 본다고 가정
        Vector2 toAttacker = (attackerPos - (Vector2)transform.position).normalized;

        // dot > 0 이면 공격자가 플레이어가 바라보는 방향에 있음
        return Vector2.Dot(facingDir, toAttacker) > 0f;
    }
}