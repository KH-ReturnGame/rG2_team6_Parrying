using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown){
            SceneManager.LoadScene("GameScene");
        }
    }
}
