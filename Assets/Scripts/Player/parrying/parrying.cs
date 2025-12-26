using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    [Header("Parry")]
    [SerializeField] private KeyCode parryKey = KeyCode.LeftShift;
    [SerializeField] private float parryCooldown = 5f;
    [SerializeField] private float parryWindowDuration = 0.75f;

    private bool canParry = true;
    private bool parryWindowActive = false;
    private float cooldownTimer = 0f;
    private float parryWindowTimer = 0f;

    public PlayerReinforceAttack code;

    [Header("Guard (Q)")]
    [SerializeField] private KeyCode guardKey = KeyCode.Q;
    [Range(0f, 1f)]
    [SerializeField] private float guardDamageMultiplier = 0.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Disable movement while holding parry key (FULL LOCK)")]
    [SerializeField] private MonoBehaviour movementScriptToDisable;
    [SerializeField] private Rigidbody2D rb;

    [Header("Animation (Optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string parryBoolName = "isparrying";

    // 완전 고정용
    private bool movementLocked = false;
    private RigidbodyConstraints2D originalConstraints;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (rb != null) originalConstraints = rb.constraints;

        canParry = true;
        parryWindowActive = false;
        cooldownTimer = 0f;
        parryWindowTimer = 0f;
        movementLocked = false;
    }

    private void Update()
    {
        HandleCooldown();
        HandleParryWindow();

        // 패링 입력(눌렀을 때 시작)
        if (Input.GetKeyDown(parryKey) && canParry && !parryWindowActive)
        {
            StartParryWindow();
        }

        // 쉬프트를 누르고 있는 동안 완전히 고정
        HandleFullLockWhileHoldingParryKey();
    }

    private void HandleFullLockWhileHoldingParryKey()
    {
        bool holding = Input.GetKey(parryKey);

        if (holding)
        {
            LockMovementFully();

            // (선택) 홀드 컨셉: 키를 떼는 순간 패링 윈도우 종료
            if (parryWindowActive && Input.GetKeyUp(parryKey))
            {
                EndParryWindow(false, null);
            }
        }
        else
        {
            // 키를 떼었고, 패링 윈도우도 끝난 상태면 이동 복구
            if (!parryWindowActive)
            {
                UnlockMovementFully();
            }
        }
    }

    private void LockMovementFully()
    {
        if (movementLocked) return;
        movementLocked = true;

        // 1) 이동 스크립트 off
        if (movementScriptToDisable != null)
            movementScriptToDisable.enabled = false;

        // 2) 물리까지 완전 고정 (넉백/밀림/미끄러짐 방지)
        if (rb != null)
        {
            originalConstraints = rb.constraints; 
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void UnlockMovementFully()
    {
        if (!movementLocked) return;
        movementLocked = false;

        // 1) 이동 스크립트 on
        if (movementScriptToDisable != null)
            movementScriptToDisable.enabled = true;

        // 2) 고정 해제
        if (rb != null)
        {
            rb.constraints = originalConstraints;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
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
                Debug.Log("Parry ready again!");
            }
        }
    }

    private void HandleParryWindow()
    {
        if (!parryWindowActive) return;

        parryWindowTimer -= Time.deltaTime;

        if (parryWindowTimer <= 0f)
        {
            EndParryWindow(false, null);
        }
    }

    private void StartParryWindow()
    {
        parryWindowActive = true;
        parryWindowTimer = parryWindowDuration;
        Debug.Log("Parry Window Opened!");

        // 패링 시작 시 즉시 완전 고정
        LockMovementFully();

        if (animator != null && !string.IsNullOrEmpty(parryBoolName))
            animator.SetBool(parryBoolName, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyAttack = other.GetComponent<EnemyAttack>();
        if (enemyAttack == null) return;

        if (parryWindowActive)
        {
            EndParryWindow(enemyAttack.canBeParried, other);
            return;
        }

        if (!canParry && Input.GetKey(guardKey) && IsAttackFromFront(other.transform.position))
        {
            enemyAttack.ApplyDamageMultiplierOnce(guardDamageMultiplier);
            Debug.Log("Guard! Damage reduced to 50% (front only).");
            return;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.playerHP--;
    }

    private void EndParryWindow(bool success, Collider2D attack)
    {
        parryWindowActive = false;

        if (animator != null && !string.IsNullOrEmpty(parryBoolName))
            animator.SetBool(parryBoolName, false);

        if (success)
        {
            Debug.Log("Parry Success!");
            var enemyAttack = (attack != null) ? attack.GetComponent<EnemyAttack>() : null;
            enemyAttack?.CancelAttack();
            if (code != null) code.RegisterParrySuccess();
        }
        else
        {
            Debug.Log("Parry Failed! Cooldown for 5s.");
            canParry = false;
            cooldownTimer = parryCooldown;

            if (GameManager.Instance != null)
                GameManager.Instance.playerHP--;
        }

        // 키를 이미 떼었다면 즉시 복구, 아직 누르고 있으면 계속 고정 유지
        if (!Input.GetKey(parryKey))
            UnlockMovementFully();
    }

    private bool IsAttackFromFront(Vector2 attackerPos)
    {
        float facingSign = (spriteRenderer != null)
            ? (spriteRenderer.flipX ? -1f : 1f)
            : Mathf.Sign(transform.localScale.x);

        if (Mathf.Approximately(facingSign, 0f)) facingSign = 1f;

        Vector2 facingDir = new Vector2(facingSign, 0f);
        Vector2 toAttacker = (attackerPos - (Vector2)transform.position).normalized;

        return Vector2.Dot(facingDir, toAttacker) > 0f;
    }
}
