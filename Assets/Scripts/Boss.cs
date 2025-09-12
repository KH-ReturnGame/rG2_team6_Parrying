using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour

{
    private bool phase2Start = false;
    public GameObject tentacle;
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
            if (GameManager.Instance.bossHP <= 60){
                phase2Start = true;
            }
            
        }}

    void FixedUpdate()
    {
       
        
    }
    IEnumerator Bossfight()
    {
        GameManager.Instance.skill = Random.Range(0, GameManager.Instance.skillnum - 1);
        switch (GameManager.Instance.skill)
        {
            case 0: //common attack
                Instantiate(tentacle, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90));
                break;


        }
        

        yield return new WaitForSeconds(2);
        StartCoroutine(Bossfight());
    }
    }

