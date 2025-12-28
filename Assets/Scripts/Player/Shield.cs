using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private string projectileTag = "Shield";
    [SerializeField] private float offsetX = 1.2f; // 플레이어 기준 방패 거리

    private SpriteRenderer parentRenderer;

    void Awake()
    {
        // 플레이어의 SpriteRenderer (flipX 확인용)
        parentRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (parentRenderer == null) return;

        Vector3 pos = transform.localPosition;

        // 플레이어 방향(flipX)에 따라 좌우 위치만 변경
        pos.x = parentRenderer.flipX
            ? -Mathf.Abs(offsetX)
            :  Mathf.Abs(offsetX);

        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(projectileTag)) return;

        Debug.Log("Shield blocked projectile");

        PlayerReinforceAttack pra =
            GetComponentInParent<PlayerReinforceAttack>();

        if (pra != null)
        {
            Debug.Log("what");
            pra.RegisterParrySuccess(2f);
        }

        Destroy(other.gameObject);
    }
}
