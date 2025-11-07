using System.Collections;
using UnityEngine;


public class Boss : MonoBehaviour

{
    private bool phase2Start = false;
    public GameObject tentacle;
    public GameObject verticle_tentacle; //1111
    public GameObject Egg;
    public Collider2D All_attack;

    void Start()
    {
        StartCoroutine(Bossfight());
    }

    // Update is called once per frame
    void Update()
    {
        {
            //보스의 현재 HP를 감지하여 60 이하가 되었다면 2페이즈 시작 신호 발생
            if (GameManager.Instance.bossHP <= 60 && !phase2Start)
            {
                phase2Start = true;
                GameManager.Instance.skillnum = 7;
                StartCoroutine(Skill_4());


            }

        }
    }

    void FixedUpdate()
    {


    }

    IEnumerator Bossfight()
    {
        GameManager.Instance.skill = Random.Range(0, GameManager.Instance.skillnum - 1);
        switch (GameManager.Instance.skill)
        {
            case 0: // tentacle summon
                skill_0();
                break;


            case 1: // vertical tentacle summon
                skill_1(1);
                break;

            case 2: // Egg summon
                for (int i = 0; i < 4; i++)
                {
                    skill_2(0);
                }

                break;

            case 3: // All_attack
                StartCoroutine(Skill_4());
                break;

            case 4: // Egg_vertical
                skill_1(5);
                skill_2(20);
            {
                
            }
                break;
        }
        
        

        yield return new WaitForSeconds(2);
        StartCoroutine(Bossfight());
    }

    void skill_0()
    {
        Instantiate(tentacle, transform.position + new Vector3(-7, -9, 0), Quaternion.Euler(0, 0, 90));
    }

    void skill_1(float a)
    { 
        verticle_tentacle.transform.localScale *= a;
        Instantiate(verticle_tentacle, GameManager.Instance.player.transform.position, Quaternion.Euler(0, 0, 0));
        verticle_tentacle.transform.localScale /= a;
    }

    void skill_2(float a)
    {
        Instantiate(Egg,
            new Vector3(Random.Range(transform.position.x, -11),
                GameManager.Instance.player.transform.position.y + a, 0), Quaternion.Euler(0, 0, 0));

    }

IEnumerator Skill_4()
    {
        
        yield return new WaitForSeconds(4);
        All_attack.enabled = true;
        yield return new WaitForSeconds(3);
        All_attack.enabled = false;
    }
    
    
}