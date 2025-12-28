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
        rigid.linearVelocity = dir * 10f;
        
    }

    void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.CompareTag("Player")){
            GameManager.Instance.playerHP -= 2f;
            Destroy(gameObject);
        }
    }
}
