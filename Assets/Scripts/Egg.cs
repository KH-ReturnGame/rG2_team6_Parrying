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
        rigid.linearVelocity = dir;
    }
}
