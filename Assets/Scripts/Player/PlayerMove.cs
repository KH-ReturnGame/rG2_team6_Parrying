using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float HP;
    public Rigidbody2D rigid;
    public bool CanDash = true;
    public bool cannotmove = false;
    
    void Start(){
        
        CanDash = true;
        rigid = GetComponent<Rigidbody2D>();
        HP = GameManager.Instance.playerHP = 10;
    }

    void Update(){
        if(Input.GetKey(KeyCode.Space) && CanDash){
            
            Debug.Log("dash~");
        }
    }

    void FixedUpdate(){
        move();
        
        
    }

    void move(){
        rigid.linearVelocityX = Input.GetAxis("Horizontal") * 10;
    }
}