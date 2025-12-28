using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float HP;
    public Rigidbody2D rigid;
    public bool CanDash = true;
    public bool cannotmove = false;
    public BossHPUI bossHPUI_2;

    public SpriteRenderer sprite;
    public Animator animator;
    
    void Start(){
        
        CanDash = true;
        rigid = GetComponent<Rigidbody2D>();
        GameManager.Instance.playerMove = true;
        HP = GameManager.Instance.playerHP = 10;
    }

    void Update(){
        if(Input.GetKey(KeyCode.Space) && CanDash){
            
            Debug.Log("dash~");
        }
    }

    void FixedUpdate(){
        if(GameManager.Instance.playerMove){
            move();
        }
        else{
            rigid.linearVelocityX = 0;
        }
        
        
        
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
