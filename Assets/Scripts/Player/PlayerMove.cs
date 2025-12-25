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
        rigid.linearVelocityX = Input.GetAxisRaw("Horizontal") * 10;

        if(Input.GetKey(KeyCode.RightArrow)){
            animator.SetBool("isWalking", true);
            sprite.flipX = false;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            animator.SetBool("isWalking", true);
            sprite.flipX = true;
        }
        else{
            animator.SetBool("isWalking", false);
        }
    }
}