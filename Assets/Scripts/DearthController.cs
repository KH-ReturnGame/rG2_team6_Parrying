using UnityEngine;
using UnityEngine.SceneManagement;

public class DearthController : MonoBehaviour
{
    void Update(){
        if(GameManager.Instance.playerHP <= 0){
            SceneManager.LoadScene("PlayerDead");
            Debug.Log("Asdf");
        }
        if(GameManager.Instance.bossHP <= 0){
            SceneManager.LoadScene("BossDead");
        }
    }
}
