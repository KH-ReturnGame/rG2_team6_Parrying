using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float HP;
    public Rigidbody2D rigid;
    public bool CanDash = true;
    
    void Start(){
        
        CanDash = true;
        rigid = GetComponent<Rigidbody2D>();
        HP = GameManager.Instance.playerHP = 100;
    }

    void Update(){
        if(Input.GetKey(KeyCode.Space) && CanDash){
            StartCoroutine(Dash());
            Debug.Log("dash~");
        }
    }

    void FixedUpdate(){
        if(CanDash){
            move();
        }
        
        
    }

    void move(){
        transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal"), 0));
        //rigid.AddForce(Vector2.right * Input.GetAxis("Horizontal") * GameManager.Instance.PlayerSpeed, ForceMode2D.Impulse);
    }

    IEnumerator Dash(){
        CanDash = false;
        Vector2 dir = Vector2.zero;
        if(Input.GetKey(KeyCode.RightArrow)){
            rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(1f);
        CanDash = true;
    }
}
