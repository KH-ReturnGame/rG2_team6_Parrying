using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackStep
    {
<<<<<<< HEAD
        public int damage = 2; // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        public float activeTime = 0.3f; // ï¿½ï¿½ï¿½ï¿½ È°ï¿½ï¿½ ï¿½Ã°ï¿½
        public float active = 0.1f; // ï¿½ï¿½ï¿½ï¿½ È°ï¿½ï¿½È­ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        public float recovery = 1f; // ï¿½Äµï¿½ï¿½ï¿½ï¿½ï¿½
    }
    [Header("ï¿½Ô·ï¿½")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f; // ï¿½ï¿½ï¿½ï¿½Å¸ ï¿½Ô·ï¿½ ï¿½ï¿½ï¿½ï¿½
}
=======
        public int damage = 2;          // °ø°Ý µ¥¹ÌÁö
        public float activeTime = 0.3f; // °ø°Ý È°¼º ½Ã°£
        public float active = 0.1f;     // °ø°Ý È°¼ºÈ­ µô·¹ÀÌ
        public float recovery = 1f;     // ÈÄµô·¹ÀÌ
    }

    [Header("ÀÔ·Â")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f;

    [Header("ÄðÅ¸ÀÓ")]
    [SerializeField] private float attackCooldown = 4f;

    private bool canAttack = true;
    private Coroutine cooldownCo;

    private AttackStep step = new AttackStep(); // ±âº» damage=2 
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
>>>>>>> parrying
