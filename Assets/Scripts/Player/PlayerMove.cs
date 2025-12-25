using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float HP;
    public Rigidbody2D rigid;
    public bool CanDash = true;
    public bool cannotmove = false;
    public SpriteRenderer sprite;
    public Animator animator;
    
    void Start(){
        
        CanDash = true;
        rigid = GetComponent<Rigidbody2D>();
        HP = GameManager.Instance.playerHP = 100;
        sprite.GetComponent<SpriteRenderer>();
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
        rigid.linearVelocityX = Input.GetAxisRaw("Horizontal") * 10;

        if(Input.GetKey(KeyCode.RightArrow)){
            sprite.flipX = false;
            animator.SetBool("isWalking", true);
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            sprite.flipX = true;
            animator.SetBool("isWalking", true);
        }
        else{
            animator.SetBool("isWalking", false);
        }
    }
}
