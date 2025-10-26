using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour

{
    private bool phase2Start = false;
    public GameObject tentacle;
    public GameObject verticle_tentacle; //1111
    public int skillnum;

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
                instantiate(verticle_tentacle, GameManager.Instance.player.transform.position , Quaternion.Euler(0, 0, 0));
                break;

        }


        yield return new WaitForSeconds(2);
        StartCoroutine(Bossfight());
    }
}