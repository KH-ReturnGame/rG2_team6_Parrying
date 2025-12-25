using UnityEngine;
using System.Collections;


public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackStep
    {
        public int damage = 2; // ���� ������
        public float activeTime = 0.3f; // ���� Ȱ�� �ð�
        public float active = 0.1f; // ���� Ȱ��ȭ ������
        public float recovery = 1f; // �ĵ�����
    }
    [Header("�Է�")]
    public KeyCode attackKey = KeyCode.E;
    public float comboBuffer = 0.2f; // ����Ÿ �Է� ����
}