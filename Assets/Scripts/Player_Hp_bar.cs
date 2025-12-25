using UnityEngine;

public class PlayerHPBinder : MonoBehaviour
{
    [SerializeField] private BossHPUI hpUI;

    private void Awake()
    {
        // 인스펙터에 안 넣었으면 같은 오브젝트/자식에서 자동으로 찾기
        if (hpUI == null)
            hpUI = GetComponentInChildren<BossHPUI>(true);
    }

    private void Start()
    {
        if (GameManager.Instance == null || hpUI == null) return;

        hpUI.Init(GameManager.Instance.playerMaxHP);
        hpUI.SetHP(GameManager.Instance.playerHP);
    }

    private void Update()
    {
        if (GameManager.Instance == null || hpUI == null) return;

        hpUI.SetHP(GameManager.Instance.playerHP);
    }
}