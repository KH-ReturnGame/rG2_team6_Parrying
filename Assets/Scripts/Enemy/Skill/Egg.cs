using System;
using UnityEngine;

public class Egg : MonoBehaviour
{
    Rigidbody2D rigid;
    public Vector3 dir;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        dir = (GameManager.Instance.player.transform.position - transform.position).normalized;
                            
    }
    
    

    void FixedUpdate(){
        Debug.Log(dir.normalized);
        transform.Translate(dir * 10 * Time.deltaTime);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject); //1
        }
    }
}
