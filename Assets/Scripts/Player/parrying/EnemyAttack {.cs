using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool canBeParried;
    public float damage;

    //다음 1회 공격에만 적용될 배율
    private float _nextDamageMultiplier = 1f;

    //ParrySystem(가드/패링 판정)에서 호출
    public void ApplyDamageMultiplierOnce(float multiplier)
    {
        _nextDamageMultiplier = Mathf.Clamp(multiplier, 0f, 1f);
    }

    //실제로 데미지를 줄 때 이 값을 사용
    public float ConsumeFinalDamage()
    {
        float finalDamage = damage * _nextDamageMultiplier;
        _nextDamageMultiplier = 1f;
        return finalDamage;
    }

    public void CancelAttack()
    {
        Debug.Log("asdfqwer");
    }
}
