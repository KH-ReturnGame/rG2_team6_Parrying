using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackStep
    {
        public int damage = 2;          // ���� ������
        public float activeTime = 0.3f; // ���� Ȱ�� �ð�
        public float active = 0.1f;     // ���� Ȱ��ȭ ������
        public float recovery = 1f;     // �ĵ�����
    }

    [Header("�Է�")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f;

    

    [Header("��Ÿ��")]
    [SerializeField] private float attackCooldown = 4f;

    private bool canAttack = true;
    private Coroutine cooldownCo;

    private AttackStep step = new AttackStep(); // �⺻ damage=2 

    public SpriteRenderer sprite;
    public Animator animator;
    private void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            Debug.Log($"Attack Damage = {step.damage}");

            canAttack = false;

            if (cooldownCo != null) StopCoroutine(cooldownCo);
            cooldownCo = StartCoroutine(AttackCooldownRoutine());
            animator.SetTrigger("normalAttack");
            GameManager.Instance.bossHP -= step.damage;
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSecondsRealtime(attackCooldown);

        canAttack = true;
        cooldownCo = null;
    }
}
