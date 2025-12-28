using UnityEngine;

public class GameManager : MonoBehaviour

{
    static public GameManager Instance;

    public GameObject boss;


    [Header("# Player")]
    public float playerHP;
    public float playerMaxHP = 10;
    public float playerATK;
    public float PP;  //parrying point
    public GameObject player;

    public bool playerDie;
    public bool playerMove = true;

    [Header("# Boss")]
    public float bossHP;
    public float bossMaxHP = 120;
    public float bossATK;
    public float bossHP2 = 60;  //phase2 hp

    public float bossCoolTime = 3;
    public bool bossDie;

    [Header("# Others")]
    public float GameTime;

    [Header("# bossSkill")] 
    public int skill;
    public int skillnum;
    public int phase1 = 3;
    public int phase2 = 3;
    

    void Awake(){
        Instance = this;
    }
    
}