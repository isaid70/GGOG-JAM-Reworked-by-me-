using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject parent;
    public GameObject[] gamblePrefabs;
    public bool CanMove { get; private set; }
    public bool CanAttack { get; private set; }
    public bool CanDash { get; private set; }

    public bool CanBossAttack { get; private set; }

    public GameState CurrentState = GameState.Combat;

    [Header("References")]
    public Transform player;
    public GameObject boss;

    [Header("Gamble Area")]
    public Transform playerGamblePoint;
    public Transform bossGamblePoint;
    public Transform gambleSpawnPoint;

    Vector3 playerOldPos;
    Vector3 bossOldPos;

    bool gambleRunning;
    BossAttacks bossAttacks;
    Gambles gambles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetCombatState();

        if (boss != null)
        {
            bossAttacks = boss.GetComponent<BossAttacks>();
        }

        gambles = GetComponent<Gambles>();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetCombatState()
    {
        CurrentState = GameState.Combat;

        CanBossAttack = true;
        CanMove = true;
        CanAttack = true;
        CanDash = true;
    }

    public void SetGambleState()
    {
        CurrentState = GameState.Gamble;

        if (bossAttacks != null)
        {
            bossAttacks.StopAttacks();
            bossAttacks.canCast = true;
        }

        CanBossAttack = false;
        CanMove = true;
        CanAttack = false;
        CanDash = false;
    }

    public void StartGamble()
    {
        if (gambleRunning)
            return;

        if (player == null || boss == null || playerGamblePoint == null || bossGamblePoint == null)
        {
            Debug.LogError("[GameManager] Gamble references are not assigned.");
            return;
        }

        StartCoroutine(GambleRoutine());
    }

    IEnumerator GambleRoutine()
    {
        gambleRunning = true;

        CurrentState = GameState.Transition;

        playerOldPos = player.position;
        bossOldPos = boss.transform.position;

        player.position = playerGamblePoint.position;
        boss.transform.position = bossGamblePoint.position;

        SetGambleState();
        if (parent != null)
        {
            parent.SetActive(true);
        }


        yield break;
    }

    public void EndGamble()
    {
        StartCoroutine(EndGambleRoutine());
    }
    IEnumerator EndGambleRoutine()
    {
        CurrentState = GameState.Transition;

        player.position = playerOldPos;
        boss.transform.position = bossOldPos;

        SetCombatState();

        gambleRunning = false;
        yield break;
    }

    public void SpawnGamble(int gamble)
    {
        switch(gamble)
            {
            case 0:
                gambles?.SpinSlot();
                break;
            case 1:
                gambles?.CoinFlip();
                break;
            case 2:
                gambles?.PlayMineFarm();
                break;
            default:
                Debug.LogError("Invalid gamble index: " + gamble);
                break;
        }
    }
}
