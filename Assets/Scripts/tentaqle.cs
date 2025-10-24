using UnityEngine;
using System.Collections;

public class tentaqle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Bossfight());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Bossfight()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
