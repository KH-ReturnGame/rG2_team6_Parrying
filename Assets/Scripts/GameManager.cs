using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    
    public GameObject boss;
    public GameObject player;
    

    [Header("# Player")]
    public float playerHP;
    public float playerMaxHP = 10;
    public float playerATK;
    public float PP;  //parrying point
    public float PlayerSpeed;
    public bool CanDash;

    public bool playerDie;

    [Header("# Boss")]
    public float bossHP;
    public float bossMaxHP = 120;
    public float bossATK;
    public float bossHP2 = 60;  //phase2 hp

    public float bossCoolTime = 3;
    public bool bossDie;

    [Header("# Others")]
    public float GameTime;


    void Awake(){
        Instance = this;
    }
}
