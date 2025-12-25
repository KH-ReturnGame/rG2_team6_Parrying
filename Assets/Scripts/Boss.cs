using System.Collections;
using UnityEngine;


public class Boss : MonoBehaviour

{
    
    private Coroutine bossLoop;
    private int lastSkill1 = -1; // 가장 최근
    private int lastSkill2 = -1;
    private int bossfightEnterCount = 0;
    private int bossfightRunCount = 0;
    private bool phase2Start = false;
    private bool isDoingPattern = false;
    public GameObject tentacle;
    public GameObject verticle_tentacle; //1111
    public GameObject Egg;
    public Collider2D All_attack;
    public GameObject Ready_motion;
    public BossHPUI bossHPUI;
    private float lastHP = -1f;

    

    IEnumerator Start()
    {
        StartBossLoop();

        yield return null; // 한 프레임 대기(초기화 순서 안정)
        if (bossHPUI != null && GameManager.Instance != null)
            bossHPUI.Init(GameManager.Instance.bossMaxHP);
    }


    private void StartBossLoop()
    {
        if (bossLoop != null) return; //  이미 돌고 있으면 시작 금지
        bossLoop = StartCoroutine(Bossfight());
    }


    // Update is called once per frame
    void Update()
    {
        {
            if (bossHPUI == null || GameManager.Instance == null) return;
            bossHPUI.SetHP(GameManager.Instance.bossHP);
        }
    }

    int GetNextSkillNoLast2()
    {
        int min = phase2Start ? 3 : 0;
        int max = phase2Start ? 7 : 3; // max 미포함

        int range = max - min;

        // 범위가 3개 미만이면 "최근 2개 금지"를 완벽히 만족할 수 없음
        // (예: 선택지가 2개인데 최근 2개를 금지하면 남는 게 0개)
        // 그래서 이 경우는 "최근 1개만 금지"로 자동 완화
        bool banLast2 = range >= 3;

        int next = Random.Range(min, max);

        if (banLast2)
        {
            while (next == lastSkill1 || next == lastSkill2)
                next = Random.Range(min, max);
        }
        else if (range >= 2)
        {
            while (next == lastSkill1)
                next = Random.Range(min, max);
        }

        // 기록 업데이트
        lastSkill2 = lastSkill1;
        lastSkill1 = next;

        return next;
    }

    IEnumerator Bossfight()
    {
        Debug.Log("[Boss] Bossfight START");

        // 첫 스킬 초기화
        GameManager.Instance.skill = GetNextSkillNoLast2();

        while (true)
        {
            //  Phase2 진입 처리 (단 1번)
            if (!phase2Start && GameManager.Instance.bossHP <= 60)
            {
                phase2Start = true;

                // 최근 기록 초기화 (권장)
                lastSkill1 = -1;
                lastSkill2 = -1;

                // 전체 공격 동안 완전 정지
                yield return StartCoroutine(All_attack_on());
                yield return new WaitForSeconds(3f);
            }

            // 🟢 패턴 1회 실행
            yield return StartCoroutine(DoPattern(GameManager.Instance.skill));

            // 🟡 다음 스킬 선택 (최근 2개 금지)
            GameManager.Instance.skill = GetNextSkillNoLast2();

            //  패턴 간 쿨타임
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator DoPattern(int skill) {

    switch (skill)
        {
            case 0: // tentacle summon
                tentacle_skill(true);
                break;


            case 1: // vertical tentacle summon
                Ready_motion.SetActive(true);
                yield return new WaitForSeconds(1f);
                vertical_tentacle_skill(2f);
               
                
                break;

            case 2: // Egg summon
                for (int i = 0; i < 4; i++)
                {
                    Egg_skill(0);
                }

                break;

            case 3: // All_attack
                yield return (All_attack_on());
                break;
            

            case 4: // Egg_vertical
                vertical_tentacle_skill(5);
                for (int i = 0; i < 6; i++) {
                    Egg_skill(20);
                }
                break;



            case 5: // vertical_tentacle * 4, tentacle_skill * 1
            {
                for (int i = 0; i < 4; i++)
                {
                    vertical_tentacle_skill(4);
                    yield return new WaitForSeconds(0.5f);
                   
                    

                }
                
                tentacle_skill(true);
                break;
            }


            case 6: // Reverse tentacle

                tentacle_skill(false);
                break;
        }
    
    }

    void tentacle_skill(bool a)
    {
        if (a)
        {
         Instantiate(tentacle, transform.position + new Vector3(-4,-5, 0), Quaternion.Euler(0, 0, 90));

            
        }
        else if(!a)
        {
            Instantiate(tentacle, transform.position + new Vector3(-6, -5, 0), Quaternion.Euler(0, 0, -90));
        }
       
        
    }

    void vertical_tentacle_skill(float a)
    { 
        var obj = Instantiate(
            verticle_tentacle, GameManager.Instance.player.transform.position, Quaternion.identity);

        obj.transform.localScale *= a;

        Destroy(obj, 1f);
    }

    void Egg_skill(float a)
    {
        Instantiate(Egg,
            new Vector3(Random.Range(transform.position.x, -8),
                GameManager.Instance.player.transform.position.y + a, 0), Quaternion.Euler(0, 0, 0));

    }

IEnumerator All_attack_on()
    {
        
        yield return new WaitForSeconds(2);
        All_attack.enabled = true;
        yield return new WaitForSeconds(3);
        All_attack.enabled = false;
    }


}