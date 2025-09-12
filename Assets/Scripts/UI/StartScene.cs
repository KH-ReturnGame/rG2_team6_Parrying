using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void OnClickStartButton(){
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickExitButton(){
        Application.Quit();
    }
}
