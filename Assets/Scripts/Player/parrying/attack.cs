using UnityEngine;
using System.Collections;


public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackStep
    {
        public int damage = 2; // 공격 데미지
        public float activeTime = 0.3f; // 공격 활성 시간
        public float active = 0.1f; // 공격 활성화 딜레이
        public float recovery = 1f; // 후딜레이
    }
    [Header("입력")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f; // 다음타 입력 버퍼
}