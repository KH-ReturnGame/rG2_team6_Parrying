using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    public System.Action OnParryReady;
    private bool canParry = true;
    private bool parryWindowActive = false; // 패링 윈도우 활성 상태
    private float parryCooldown = 5f;  // 쿨다운 시간
    private float cooldownTimer = 0f;
    private float parryWindowDuration = 0.75f; // 패링 윈도우 지속 시간 (0.75초)
    private float parryWindowTimer = 0f;
    public System.Action OnParrySuccess;

    public PlayerReinforceAttack code;
    public Animator animator;
    
    
    
    //가드(데미지 감소) 설정 
    [Header("Guard (Q)")]
    [SerializeField] private KeyCode guardKey = KeyCode.Q;
    [Range(0f, 1f)]
    [SerializeField] private float guardDamageMultiplier = 0.5f; // 50% 데미지
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public float CooldownNormalized
    {
        get
        {
            // 준비 상태면 1(가득 참)
            if (canParry) return 1f;

            // cooldownTimer는 5 -> 0으로 감소하니까 0~1로 변환
            return Mathf.Clamp01(1f - (cooldownTimer / parryCooldown));
        }
    }

    public bool CanParry => canParry;
    public float ParryCooldownSeconds => parryCooldown;
    public float CooldownRemainingSeconds => canParry ? 0f : Mathf.Max(0f, cooldownTimer);

    
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
                cooldownTimer = 0f;
                Debug.Log("PARry READY!");
                OnParryReady?.Invoke();   // ✅ 여기
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
        animator.SetTrigger("parrying");
        GameManager.Instance.playerMove = false;
        
        canParry = false;
        cooldownTimer = parryCooldown;
        
        parryWindowActive = true;
        parryWindowTimer = parryWindowDuration;
        
        Debug.Log("Parry Window Opened!");
    }

    private void OnTriggerEnter2D(Collider2D attack)
    {
        Debug.Log("I don't konw this log");
        var enemyAttack = attack.GetComponent<EnemyAttack>();
        if (enemyAttack == null) return;
        Debug.Log("I love you");
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
        GameManager.Instance.playerHP--;

    }

    private void EndParryWindow(bool success, Collider2D attack = null)
    {
        parryWindowActive = false;

        if (success)
        {
            var enemyAttack = attack != null ? attack.GetComponent<EnemyAttack>() : null;
            enemyAttack?.CancelAttack();

            if (code != null) code.RegisterParrySuccess();

            // ✅ 성공하면 쿨타임 즉시 초기화(바 100%)
            canParry = true;
            cooldownTimer = 0f;

            // (선택) 성공 연출 이벤트 쏘기 — 아래 2번에서 씀
            OnParrySuccess?.Invoke();
        }
        else
        {
            // 실패하면 이미 쿨타임 돌고 있으니 유지
            Debug.Log("Parry Failed!");
            GameManager.Instance.playerHP--;
        }

        animator.SetTrigger("endparrying");
        GameManager.Instance.playerMove = true;
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