using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            SceneManager.LoadScene("GameScene");
        }
    }
}
