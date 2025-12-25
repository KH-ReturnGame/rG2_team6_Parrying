using UnityEngine;
using UnityEngine.Events;

public class PlayerReinforceAttack : MonoBehaviour
{
    [Header("Key Setting")]
    [SerializeField] private KeyCode reinforceKey = KeyCode.Q;

    [Header("Parry Count")]
    [SerializeField] private int parryNeeded = 0;
    [SerializeField] private int currentParryCount = 0;

    [Header("Reinforce Attack")]
    [SerializeField] private bool reinforceReady = false;
    [SerializeField] private int reinforceDamage = 10;

    [Header("Optional Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string reinforceAttackTrigger = "ReinforceAttack";

    [Header("Optional Event Hook")]
    // 강화공격 발동 시 데미지 값(int)을 넘겨줍니다.
    public UnityEvent<int> OnReinforceAttack;

    public int CurrentParryCount => currentParryCount;
    public int ParryNeeded => parryNeeded;
    public bool ReinforceReady => reinforceReady;

    // 누적된 "막은 공격력" (패링 성공 시 적 공격력(damage 등)을 누적)
    [Header("Accumulated Blocked Attack Power")]
    [SerializeField] private float accumulatedBlockedAttackPower = 0f;

    private void Update()
    {
        //Q 대신 serialize된 키 사용
        if (Input.GetKeyDown(reinforceKey))
        {
            TryUseReinforceAttack();
        }
    }

    // 패링 성공이 판정되어지는 순간 이 함수를 누적 시킵니다. (기존 호출 호환)
    public void RegisterParrySuccess()
    {
        RegisterParrySuccess(0f);
    }

    // 패링 성공 시 "막은 공격력"까지 함께 누적시킴
    public void RegisterParrySuccess(float blockedAttackPower)
    {
        if (reinforceReady) return;

        // 막은 공격력 누적 (음수 방지)
        if (blockedAttackPower > 0f)
            accumulatedBlockedAttackPower += blockedAttackPower;

        currentParryCount++;

        if (currentParryCount >= parryNeeded)
        {
            ChargeReinforceAttack();
        }
    }

    // Q키가 입력이 된다면은 이 할수를 사용하여 강회공격을 합니다.
    public bool TryUseReinforceAttack()
    {
        if (!reinforceReady) return false;

        // 강화 공격기 대미지 = 4 + 5.6 * sqrt(누적된 막은 공격력)
        reinforceDamage = CalculateReinforceDamage(accumulatedBlockedAttackPower);

        Debug.Log($"Reinforce Attack Damage = {reinforceDamage}");

        if (animator != null && !string.IsNullOrEmpty(reinforceAttackTrigger))
            animator.SetTrigger(reinforceAttackTrigger);

        OnReinforceAttack?.Invoke(reinforceDamage);

        // ✅ [추가] 사용 후 상태 초기화(다음 강화공격을 다시 모으게 함)
        ResetReinforceState();

        // ✅ [추가] 성공적으로 사용했으니 true 반환
        return true;
    }

    private void ChargeReinforceAttack()
    {
        reinforceReady = true;
        currentParryCount = parryNeeded;
    }

    private void ResetReinforceState()
    {
        reinforceReady = false;
        currentParryCount = 0;
        accumulatedBlockedAttackPower = 0f; //  누적 막은 공격력도 초기화
    }

    //  데미지 계산 함수(정수 반환)
    private int CalculateReinforceDamage(float blockedPower)
    {
        float dmg = 4f + 5.6f * Mathf.Sqrt(Mathf.Max(0f, blockedPower));
        return Mathf.RoundToInt(dmg); // 반올림 (원하면 FloorToInt/CeilToInt로 변경 가능)
    }
}
