using UnityEngine;
using System.Collections; // 이게 있어야 코루틴 사용 가능

public class Vertic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Delay());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
