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

    public GameObject particle;

    // 누적된 "막은 공격력" (패링 성공 시 적 공격력(damage 등)을 누적)
    [Header("Accumulated Blocked Attack Power")]
    [SerializeField] private float accumulatedBlockedAttackPower = 0f;

    //  패링을 1번이라도 성공해야 강화공격 사용 가능(잠금 해제)
    [Header("Lock")]
    [SerializeField] private bool hasParriedAtLeastOnce = false;

    private void Update()
    {

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
        //패링 성공 1회라도 발생하면 잠금 해제
        hasParriedAtLeastOnce = true;

        if (reinforceReady) return;

        // 막은 공격력 누적 (음수 방지)
        if (blockedAttackPower > 0f)
            accumulatedBlockedAttackPower += blockedAttackPower;

        // parryNeeded가 0 이하라도 "패링 1번"은 필요.
        // 패링 성공하면 즉시 충전되게 처리.
        if (parryNeeded <= 0)
        {
            ChargeReinforceAttack();
            return;
        }

        currentParryCount++;

        if (currentParryCount >= parryNeeded)
        {
            ChargeReinforceAttack();
        }
    }

    // Q키가 입력이 된다면은 이 할수를 사용하여 강회공격을 합니다.
    public bool TryUseReinforceAttack()
    {
        // 패링 0회면 사용 불가
        if (!hasParriedAtLeastOnce) return false;

        if (!reinforceReady) return false;

        // 강화 공격기 대미지 = 4 + 5.6 * sqrt(누적된 막은 공격력)
        reinforceDamage = CalculateReinforceDamage(accumulatedBlockedAttackPower);

        Debug.Log($"Reinforce Attack Damage = {reinforceDamage}");
        

        if (animator != null && !string.IsNullOrEmpty(reinforceAttackTrigger))
            animator.SetTrigger(reinforceAttackTrigger);
            GameManager.Instance.playerMove = false;
            GameManager.Instance.player.GetComponent<SpriteRenderer>().flipX = false;

        OnReinforceAttack?.Invoke(reinforceDamage);
        
        // 사용 후 상태 초기화(다음 강회공격을 다시 모으게 함)
        ResetReinforceState();
        
        GameManager.Instance.playerMove = true;
        
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
        accumulatedBlockedAttackPower = 0f;
        // hasParriedAtLeastOnce는 초기화하지 않음:
    }

    // 데미지 계산 함수(정수 반환)
    private int CalculateReinforceDamage(float blockedPower)
    {
        float dmg = 4f + 5.6f * Mathf.Sqrt(Mathf.Max(0f, blockedPower));
        return Mathf.RoundToInt(dmg);
    }

    void instantiate(){
        Instantiate(particle, transform.position + new Vector3(10, -1, 0), Quaternion.Euler(0, 0, 0));
        GameManager.Instance.bossHP -= reinforceDamage;
    }
}
