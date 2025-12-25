using UnityEngine;
using System.Collections;

public class tentaqle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(tentaqle_skill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator tentaqle_skill()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
