using System.Collections;
using UnityEngine;

public class Ready_motion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(Delay(3f));
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
