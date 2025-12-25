using UnityEngine;
using UnityEngine.Events;

public class PlayerReinforceAttack : MonoBehaviour
{
    [Header("Key Setting")]
    [SerializeField] private KeyCode reinforceKey = KeyCode.Q;

    [Header("Parry Count")]
    [SerializeField] private int parryNeeded = 5;
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

    private void Update()
    {
        if (Input.GetKeyDown(reinforceKey))
        {
            TryUseReinforceAttack();
        }
    }

    // 패링 성공이 판정되어지는 순간 이 함수를 누적 시킵니다.
    public void RegisterParrySuccess()
    {
        if (reinforceReady) return;

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

        if (animator != null && !string.IsNullOrEmpty(reinforceAttackTrigger))
        {
            animator.SetTrigger(reinforceAttackTrigger);
        }

        OnReinforceAttack?.Invoke(reinforceDamage);

        ResetReinforceState();
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
    }
}