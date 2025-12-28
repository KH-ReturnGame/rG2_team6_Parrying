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
    public GameObject All_attack;
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
        int min = phase2Start ? 2 : 0;
        int max = phase2Start ? 7 : 3; 

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
            case 0:
            {
                Vector2 attackPos = new Vector2(0f, 0f); // tentacle_skill이 쓰는 위치와 동일
                Vector2 size = GetColliderSize(tentacle);
                float warnTime = 0.8f;

                yield return StartCoroutine(ShowDangerRect(attackPos, size, warnTime));
                tentacle_skill(true);
                break;
            }



            case 1:
            {
                Vector2 attackPos = GameManager.Instance.player.transform.position + new Vector3(0, -2, 0);

                
                // 실제 공격
                vertical_tentacle_skill(2f);
                break;
            }


            case 2:
            {
                float warnTime = 0.6f;
                Vector2 eggSize = GetColliderSize(Egg);

                for (int i = 0; i < 4; i++)
                {
                    // ✅ 실제 생성될 좌표를 미리 결정
                    Vector2 spawnPos = new Vector2(
                        Random.Range(transform.position.x, -8),
                        GameManager.Instance.player.transform.position.y + 0f
                    );

                    // 예고
                    yield return StartCoroutine(ShowDangerRect(spawnPos, eggSize, warnTime));

                    // 실제 생성(좌표를 직접 넘겨서 일치 보장)
                    Instantiate(Egg, spawnPos, Quaternion.identity);
                }
                break;
            }

            case 3:
            {
                Collider2D col = All_attack.GetComponent<Collider2D>();
                Vector2 pos = col.bounds.center;
                Vector2 size = col.bounds.size;

                yield return StartCoroutine(
                    ShowDangerRect(pos, size, 1.2f)
                );

                yield return StartCoroutine(All_attack_on());
                break;
            }

            

            case 4: // Egg_vertical
                vertical_tentacle_skill(5);
                for (int i = 0; i < 6; i++) {
                    Egg_skill(20);
                }
                break;



            case 5:
            {
                float warnTime = 0.7f;

                // 세로 촉수 4회
                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = (Vector2)GameManager.Instance.player.transform.position + new Vector2(0f, -2f);
                    Vector2 size = GetColliderSize(verticle_tentacle) * 4f; // 네 스케일 반영

                    yield return StartCoroutine(ShowDangerRect(pos, size, warnTime));
                    vertical_tentacle_skill(4f);

                    yield return new WaitForSeconds(0.2f); // 패턴 템포(원하면)
                }

                // 마지막 촉수 1회
                Vector2 tpos = new Vector2(0f, 0f);
                Vector2 tsize = GetColliderSize(tentacle);

                yield return StartCoroutine(ShowDangerRect(tpos, tsize, 0.8f));
                tentacle_skill(true);

                break;
            }

        }
    
    }

    void tentacle_skill(bool a)
    {
         Instantiate(tentacle,new Vector3(0, 0f, 0), Quaternion.Euler(0, 0, 0));

            
       
        
    }

    void vertical_tentacle_skill(float a)
    { 
        var obj = Instantiate(
            verticle_tentacle, GameManager.Instance.player.transform.position + new Vector3(0, -2, 0), Quaternion.identity);

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
        All_attack.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        All_attack.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.9f);
        All_attack.GetComponent<Collider2D>().enabled = false;
        All_attack.SetActive(false);
    }
[SerializeField] private GameObject dangerRectPrefab;

IEnumerator ShowDangerRect(Vector2 pos, Vector2 worldSize, float duration)
{
    GameObject danger = Instantiate(dangerRectPrefab, pos, Quaternion.identity);

    // ✅ bounds가 0으로 잡히는 경우 방지: 1프레임 대기
    yield return null;

    var sr = danger.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        // ✅ 스프라이트 월드 크기(0 방지)
        Vector2 spriteSize = sr.bounds.size;

        // bounds가 0이면 (Sprite 미할당/비활성/정렬 문제 등)
        if (spriteSize.x <= 0.0001f || spriteSize.y <= 0.0001f)
        {
            // 안전값: 그냥 worldSize를 localScale로 쓰지 말고 적당히 키우기
            danger.transform.localScale = new Vector3(
                Mathf.Max(worldSize.x, 1f),
                Mathf.Max(worldSize.y, 1f),
                1f
            );
        }
        else
        {
            Vector2 scale = new Vector2(
                worldSize.x / spriteSize.x,
                worldSize.y / spriteSize.y
            );

            // ✅ 0 스케일 방지(최소값)
            scale.x = Mathf.Max(scale.x, 0.01f);
            scale.y = Mathf.Max(scale.y, 0.01f);

            danger.transform.localScale = new Vector3(scale.x, scale.y, 1f);
        }

        // ✅ 항상 위에 보이게
        sr.sortingOrder = 100;
    }

    yield return new WaitForSeconds(duration);
    Destroy(danger);
}



Vector2 GetColliderSize(GameObject prefabOrObj)
{
    var col = prefabOrObj.GetComponentInChildren<Collider2D>(true);
    return col != null ? (Vector2)col.bounds.size : Vector2.one;
}

Vector2 GetPrefabColliderSizeSafe(GameObject prefab)
{
    var temp = Instantiate(prefab, new Vector3(9999, 9999, 0), Quaternion.identity);

    // 1프레임 기다리지 않아도 되는 경우가 대부분이지만
    // 안정성을 원하면 코루틴으로 분리 가능
    var col = temp.GetComponentInChildren<Collider2D>();

    Vector2 size = col != null ? (Vector2)col.bounds.size : Vector2.one;

    Destroy(temp);
    return size;
}


}