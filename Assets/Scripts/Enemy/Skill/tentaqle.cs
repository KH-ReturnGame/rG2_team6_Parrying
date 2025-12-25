using UnityEngine;
using System.Collections;

public class tentaqle : MonoBehaviour
{
    public Collider2D coll;
    void Start()
    {
        StartCoroutine(tentaqle_skill());
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator tentaqle_skill()
    {
        yield return new WaitForSeconds(1.1f);
        coll.enabled = true;
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }
}
