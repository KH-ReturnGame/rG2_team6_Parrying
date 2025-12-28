using UnityEngine;

public class ParryUIBinder : MonoBehaviour
{
    [SerializeField] private ParrySystem parry;
    [SerializeField] private BossHPUI barUI;

    private void Start()
    {
        if (parry == null) parry = FindFirstObjectByType<ParrySystem>();
        if (barUI != null) barUI.Init(1f);

        if (parry != null)
            parry.OnParrySuccess += HandleParryReady;
    }

    private void OnDestroy()
    {
        if (parry != null)
            parry.OnParryReady -= HandleParryReady;
    }  

    private void Update()
    {
        if (parry == null || barUI == null) return;
        barUI.SetHP(parry.CooldownNormalized);
    }
    
    private void HandleParryReady()
    {
        Debug.Log("ParryUIBinder RECEIVED READY EVENT");
        if (barUI != null) barUI.PlayReadyFx();
        
    }
}