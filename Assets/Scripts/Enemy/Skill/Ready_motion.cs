using System.Collections;
using UnityEngine;

public class Ready_motion : MonoBehaviour
{
    private Coroutine co;

    private void OnEnable()
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(DisableAfter(2f));
    }

    private IEnumerator DisableAfter(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        co = null;
    }
}