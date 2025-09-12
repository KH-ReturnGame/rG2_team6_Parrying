using UnityEngine;

public class ParrySystem : MonoBehaviour
{

    private float parryCooldown = 5f;       // ��ٿ� �ð�
    private float parryWindowDuration = 0.25f; // �и� ������ ���� �ð� (0.25��)

    public bool canParry;
    public bool parryWindowActive;
    public float cooldownTimer;
    public float parryWindowTimer;

    void awake()
    {
        // �ʱ�ȭ
        canParry = true;
        parryWindowActive = false;
        cooldownTimer = 0f;
        parryWindowTimer = 0f;
    }
    void Update()
    {
        HandleCooldown();
        HandleParryWindow();

        // �и� �Է�
        if (Input.GetKeyDown(KeyCode.LeftShift) && canParry)
        {
            StartParryWindow();
        }
    }

    /// <summary>
    /// ��ٿ� ����
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
    /// �и� ������ ����
    /// </summary>
    private void HandleParryWindow()
    {
        if (parryWindowActive)
        {
            parryWindowTimer -= Time.deltaTime;

            // ������ ������ �ڵ� ���� ó��
            if (parryWindowTimer <= 0f)
            {
                EndParryWindow(false);
            }
        }
    }

    /// <summary>
    /// �и� �õ� ���� �� ª�� �ð��� �и� ����
    /// </summary>
    private void StartParryWindow()
    {
        parryWindowActive = true;
        parryWindowTimer = parryWindowDuration;
        Debug.Log("Parry Window Opened!");
    }

    /// <summary>
    /// ������ ���� �� ȣ��Ǵ� �Լ�
    /// (���� ���� �ý��ۿ��� ���� Ÿ�̹� ���� ȣ�� �ʿ�)
    /// </summary>
    public void OnCollision2D(EnemyAttack attack)
    {
        if (parryWindowActive && attack.canBeParried)
        {
            // ����
            EndParryWindow(true, attack);
        }
        else
        {
            // ����
            EndParryWindow(false);
        }
    }

    /// <summary>
    /// �и� ������ ����
    /// </summary>
    private void EndParryWindow(bool success, EnemyAttack attack = null)
    {
        parryWindowActive = false;

        if (success)
        {
            Debug.Log("Parry Success!");
            attack?.CancelAttack();
        }
        else
        {
            Debug.Log("Parry Failed! Cooldown for 5s.");
            canParry = false;
            cooldownTimer = parryCooldown;
        }
    }
}

/// <summary>
/// ���� ��ü ����
/// </summary>
