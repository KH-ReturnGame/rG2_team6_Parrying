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
            if (GameManager.Instance.bossHP <= 60)
            {
                phase2Start = true;
                GameManager.Instance.skillnum = 7;
                StartCoroutine(Skill_4_wait());

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
                Instantiate(tentacle, transform.position + new Vector3(-7, -9, 0), Quaternion.Euler(0, 0, 90));
                break;


            case 1: // vertical tentacle summon
                Instantiate(verticle_tentacle, GameManager.Instance.player.transform.position , Quaternion.Euler(0, 0, 0));
                break;

            case 2: // Egg summon
                for (int i = 0; i < 4; i++)
            {   
                Instantiate(Egg,
                    new Vector3(Random.Range(transform.position.x, -11),
                        GameManager.Instance.player.transform.position.y, 0), Quaternion.Euler(0, 0, 0));
               
            } break;
            
            case 3: // All_attack
                StartCoroutine(Skill_4_wait());
                break;
        }


        yield return new WaitForSeconds(2);
        StartCoroutine(Bossfight());
    }

    IEnumerator Skill_4_wait()
    {
        
        yield return new WaitForSeconds(4);
        All_attack.enabled = true;
    }
    
    
}