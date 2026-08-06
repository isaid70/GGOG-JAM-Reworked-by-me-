using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class bossSkills : MonoBehaviour
{
    playerMain player;

    public AudioSource sourceslot;
    public AudioClip clipslot;
    public AudioSource sourceyazi;
    public AudioClip clipyazi;

    statSystem statSystem;

    public GameObject[] slotObjes;
    public Sprite[] slotSprites;


    Animator coinAnim;
    public GameObject coinF;
    public int coinFlipRes;
    public float animDuration = 1f;
    public Sprite[] flipSprites;


    public int[,] mineFarm = new int[3,3];
    //    public int mineCount, winCount;
    public GameObject[] mineFarmObjSolo;
    public GameObject[,] mineFarmObj = new GameObject[3,3];
    public GameObject Mine;
    public Transform yer;
    public Sprite minedef;


    public GameObject magicPrefab;
    public GameObject shin;
    public bool cheat;



    private void Start()
    {
        player = FindAnyObjectByType<playerMain>();
        statSystem = GetComponent<statSystem>();
    }




    public void CoinFlip() 
    {
        coinF.SetActive(true);
        sourceyazi.clip = clipyazi;
        sourceyazi.Play();

        coinAnim = coinF.GetComponent<Animator>();
        StartCoroutine(FlipRoutine());

        
    }

    IEnumerator FlipRoutine()
    {
        if (cheat)
        {
            for (int i = 0; i < 2; i++)
            {
                coinAnim.enabled = true;
                yield return new WaitForSeconds(animDuration);
            }
            coinAnim.enabled = false;
            coinFlipRes = 0;
            coinF.GetComponent<SpriteRenderer>().sprite = flipSprites[1];

            Vector3 centerPos = player.transform.position;

            // 4 yönde pozisyonlar
            Vector3[] spawnPositions = new Vector3[]
            {
            centerPos + new Vector3(0, 1, 0),   // Yukarý
            centerPos + new Vector3(0, -1, 0),  // Aþaðý
            centerPos + new Vector3(1, 0, 0),   // Saða
            centerPos + new Vector3(-1, 0, 0)   // Sola
            };

            foreach (Vector3 spawnPos in spawnPositions)
            {
                // Spawnla
                GameObject spawned = Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                StartCoroutine(dest(spawned));

                // Karaktere doðru dönsün
                Vector3 direction = centerPos - spawnPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawned.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                coinAnim.enabled = true;
                yield return new WaitForSeconds(animDuration);
            }
            coinAnim.enabled = false;
            coinFlipRes = Random.Range(0, 2);
            if (coinFlipRes == 0)
            {
                coinF.GetComponent<SpriteRenderer>().sprite = flipSprites[0];
                shin.SetActive(true);
            }
            else
            {
                coinF.GetComponent<SpriteRenderer>().sprite = flipSprites[1];
                Vector3 centerPos = player.transform.position;

                // 4 yönde pozisyonlar
                Vector3[] spawnPositions = new Vector3[]
                {
            centerPos + new Vector3(0, 1, 0),   // Yukarý
            centerPos + new Vector3(0, -1, 0),  // Aþaðý
            centerPos + new Vector3(1, 0, 0),   // Saða
            centerPos + new Vector3(-1, 0, 0)   // Sola
                };

                foreach (Vector3 spawnPos in spawnPositions)
                {
                    // Spawnla
                    GameObject spawned = Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                    StartCoroutine(dest(spawned));

                    // Karaktere doðru dönsün
                    Vector3 direction = centerPos - spawnPos;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spawned.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
        StartCoroutine(durat(coinF));
    }

    public void SpinSlot()
    {
        slotObjes[0].SetActive(true);
        slotObjes[0].GetComponent<Animator>().enabled = true;
        StartCoroutine(Spin());
    }
    IEnumerator Spin()
    {
        sourceslot.clip = clipslot;
        sourceslot.Play();

        float duration = 1.2f;
        float timer = 0f;
        while (timer < duration) {
            slotObjes[1].GetComponent<SpriteRenderer>().sprite = slotSprites[Random.Range(0, slotSprites.Length)];
            slotObjes[2].GetComponent<SpriteRenderer>().sprite = slotSprites[Random.Range(0, slotSprites.Length)];
            slotObjes[3].GetComponent<SpriteRenderer>().sprite = slotSprites[Random.Range(0, slotSprites.Length)];

            timer += 0.1f;
            yield return new WaitForSeconds(0.3f);
        }
        if (slotObjes[1].GetComponent<SpriteRenderer>().sprite == slotObjes[2].GetComponent<SpriteRenderer>().sprite && slotObjes[1].GetComponent<SpriteRenderer>().sprite == slotObjes[3].GetComponent<SpriteRenderer>().sprite)
        {
            Vector3 centerPos = player.transform.position;

            // 4 yönde pozisyonlar
            Vector3[] spawnPositions = new Vector3[]
            {
            centerPos + new Vector3(0, 1, 0),   // Yukarý
            centerPos + new Vector3(0, -1, 0),  // Aþaðý
            centerPos + new Vector3(1, 0, 0),   // Saða
            centerPos + new Vector3(-1, 0, 0)   // Sola
            };

            foreach (Vector3 spawnPos in spawnPositions)
            {
                // Spawnla
                GameObject spawned = Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                StartCoroutine(dest(spawned));

                // Karaktere doðru dönsün
                Vector3 direction = centerPos - spawnPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawned.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            if (cheat)
            {
                slotObjes[1].GetComponent<SpriteRenderer>().sprite = slotSprites[0];
                slotObjes[2].GetComponent<SpriteRenderer>().sprite = slotSprites[0];
                slotObjes[3].GetComponent<SpriteRenderer>().sprite = slotSprites[0];

                Vector3 centerPos = player.transform.position;

                // 4 yönde pozisyonlar
                Vector3[] spawnPositions = new Vector3[]
                {
            centerPos + new Vector3(0, 1, 0),   // Yukarý
            centerPos + new Vector3(0, -1, 0),  // Aþaðý
            centerPos + new Vector3(1, 0, 0),   // Saða
            centerPos + new Vector3(-1, 0, 0)   // Sola
                };

                foreach (Vector3 spawnPos in spawnPositions)
                {
                    // Spawnla
                    GameObject spawned = Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                    StartCoroutine(dest(spawned));

                    // Karaktere doðru dönsün
                    Vector3 direction = centerPos - spawnPos;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spawned.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
            else
            {
                Vector3 centerPos = player.transform.position;

                // 4 yönde pozisyonlar
                Vector3[] spawnPositions = new Vector3[]
                {
            centerPos + new Vector3(0, 1, 0),   // Yukarý
            centerPos + new Vector3(0, -1, 0),  // Aþaðý
            centerPos + new Vector3(1, 0, 0),   // Saða
            centerPos + new Vector3(-1, 0, 0)   // Sola
                };

                foreach (Vector3 spawnPos in spawnPositions)
                {
                    // Spawnla
                    GameObject spawned = Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                    StartCoroutine(dest(spawned));

                    // Karaktere doðru dönsün
                    Vector3 direction = centerPos - spawnPos;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spawned.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
        StartCoroutine(durat(slotObjes[0]));

    }

    public void PlayMineFarm()
    {
        Mine.SetActive(true);
        Debug.Log("Farm");
        SetUpMineObjs();
        SetUpMineFarm();
    }

    void SetUpMineObjs()
    {
        int hand = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mineFarmObj[i, j] = mineFarmObjSolo[hand];
                hand++;
            }
        }
    }

    public void SetUpMineFarm()
    {
        int col, sel;
        col = Random.Range(0, 3);
        sel = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (i == col && j == sel)
                {
                    mineFarm[i, j] = 0;
                    mineFarmObj[i, j].GetComponent<MineAreas>().var = 0;
                    mineFarmObj[i, j].GetComponent<SpriteRenderer>().sprite = minedef;
                }
                else
                {
                    mineFarm[i, j] = 1;
                    mineFarmObj[i, j].GetComponent<MineAreas>().var = 1;
                    mineFarmObj[i, j].GetComponent<SpriteRenderer>().sprite = minedef;
                }

            }
        }
    }
    IEnumerator durat(GameObject o)
    {
        yield return new WaitForSeconds(1f);
        o.SetActive(false);
    }
    IEnumerator dest(GameObject o)
    {
        yield return new WaitForSeconds(2f);
        Destroy(o);
    }
}
