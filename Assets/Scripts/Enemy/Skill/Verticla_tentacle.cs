using UnityEngine;
using System.Collections; // 이게 있어야 코루틴 사용 가능

public class Vertic : MonoBehaviour
{
    public Collider2D coll;
    void Start()
    {
        StartCoroutine(Delay());
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        coll.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
