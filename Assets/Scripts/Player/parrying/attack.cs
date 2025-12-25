using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackStep
    {
        public int damage = 2;          // 공격 데미지
        public float activeTime = 0.3f; // 공격 활성 시간
        public float active = 0.1f;     // 공격 활성화 딜레이
        public float recovery = 1f;     // 후딜레이
    }

    [Header("입력")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f;

    [Header("쿨타임")]
    [SerializeField] private float attackCooldown = 4f;

    private bool canAttack = true;
    private Coroutine cooldownCo;

    private AttackStep step = new AttackStep(); // 기본 damage=2 
    private void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            Debug.Log($"Attack Damage = {step.damage}");

            canAttack = false;

            if (cooldownCo != null) StopCoroutine(cooldownCo);
            cooldownCo = StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSecondsRealtime(attackCooldown);

        canAttack = true;
        cooldownCo = null;
    }
}
