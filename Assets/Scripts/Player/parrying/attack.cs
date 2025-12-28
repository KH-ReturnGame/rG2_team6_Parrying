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
    [SerializeField] private float attackCooldown = 1f;

    private bool canAttack = true;
    public bool inrange = false;
    private Coroutine cooldownCo;

    private AttackStep step = new AttackStep(); // �⺻ damage=2 

    public SpriteRenderer sprite;
    public Animator animator;
    private void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack && inrange)
        {
            Debug.Log($"Attack Damage = {step.damage}");

            canAttack = false;
            GameManager.Instance.playerMove = false;

            if (cooldownCo != null) StopCoroutine(cooldownCo);
            cooldownCo = StartCoroutine(AttackCooldownRoutine());
            animator.SetTrigger("normalAttack");
            GameManager.Instance.bossHP -= step.damage;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Range")){
            inrange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Range")){
            inrange = false;
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSecondsRealtime(attackCooldown);

        canAttack = true;
        cooldownCo = null;
        GameManager.Instance.playerMove = true;
    }
}
