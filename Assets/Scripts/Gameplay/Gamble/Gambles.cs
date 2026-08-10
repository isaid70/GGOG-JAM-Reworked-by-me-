using System.Collections;
using UnityEngine;

public class Gambles : MonoBehaviour
{
    public float gambleFinishTime = 2f;

    [Header("Audio")]
    public AudioSource sourceslot;
    public AudioClip clipslot;
    public AudioSource sourceyazi;
    public AudioClip clipyazi;

    [Header("Slot")]
    public GameObject[] slotObjes;
    public Sprite[] slotSprites;
    [SerializeField]private CasinoSlotMachine slotMachine;

    [Header("Coin Flip")]
    public GameObject coinF;
    public int coinFlipRes;
    public float animDuration = 1f;
    public Sprite[] flipSprites;
    public int selectedSide = 0;

    [Header("Mine Farm")]
    public int[,] mineFarm = new int[3, 3];
    public GameObject[] mineFarmObjSolo;
    public GameObject[,] mineFarmObj = new GameObject[3, 3];
    public GameObject Mine;
    public Sprite minedef;

    private PlayerMovement playerMove;
    private PlayerHealth playerHp;
    private Animator coinAnim;
    private SpriteRenderer coinRenderer;

    private void Start()
    {
        ResolvePlayerReferences();
        CacheCoinReferences();
    }

    public void CoinFlip()
    {
        ResolvePlayerReferences();
        CacheCoinReferences();

        if (coinF == null || coinAnim == null || coinRenderer == null || flipSprites == null || flipSprites.Length == 0)
        {
            GameManager.Instance?.EndGamble();
            return;
        }

        coinF.SetActive(true);
        PlayAudio(sourceyazi, clipyazi);
        StartCoroutine(FlipRoutine());
    }

    public void SpinSlot()
    {
        ResolvePlayerReferences();

        if (slotObjes == null || slotObjes.Length < 4 || slotSprites == null || slotSprites.Length == 0)
        {
            GameManager.Instance?.EndGamble();
            return;
        }

        slotObjes[0].SetActive(true);

        if (slotObjes[0].TryGetComponent(out Animator slotAnimator))
        {
            slotAnimator.enabled = true;
        }

        StartCoroutine(slotMachine.SpinRoutine());
    }

    public void PlayMineFarm()
    {
        if (Mine == null || mineFarmObjSolo == null || mineFarmObjSolo.Length < 9)
        {
            GameManager.Instance?.EndGamble();
            return;
        }

        Mine.SetActive(true);
        SetUpMineObjs();
        SetUpMineFarm();
    }

    public void SetUpMineFarm()
    {
        int mineColumn = Random.Range(0, 3);
        int mineRow = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                bool hasMine = i == mineColumn && j == mineRow;
                mineFarm[i, j] = hasMine ? 0 : 1;
                ConfigureMineCell(mineFarmObj[i, j], mineFarm[i, j]);
            }
        }
    }

    private IEnumerator FlipRoutine()
    {
        for (int i = 0; i < 2; i++)
        {
            coinAnim.enabled = true;
            yield return new WaitForSeconds(animDuration);
        }

        coinAnim.enabled = false;
        coinFlipRes = Random.Range(0, 2);
        coinRenderer.sprite = flipSprites[coinFlipRes];

        if (playerHp != null)
        {
            if (coinFlipRes != selectedSide)
            {
                playerHp.GetDamage(20);
            }
            else
            {
                playerHp.GetHeal(30);
            }
        }

        StartCoroutine(FinishGambleAfterDelay(coinF));
    }

    private IEnumerator SpinRoutine()
    {
        PlayAudio(sourceslot, clipslot);

        float duration = 1.2f;
        float timer = 0f;

        while (timer < duration)
        {
            SetSlotSprite(1, GetRandomSlotSprite());
            SetSlotSprite(2, GetRandomSlotSprite());
            SetSlotSprite(3, GetRandomSlotSprite());

            timer += 0.1f;
            yield return new WaitForSeconds(0.3f);
        }

        ApplySlotReward();
        StartCoroutine(FinishGambleAfterDelay(slotObjes[0]));
    }

    private void ApplySlotReward()
    {
        Sprite first = GetSlotSprite(1);
        Sprite second = GetSlotSprite(2);
        Sprite third = GetSlotSprite(3);

        if (playerMove == null || first == null || first != second || first != third)
        {
            return;
        }

        for (int i = 0; i < slotSprites.Length; i++)
        {
            if (first == slotSprites[i])
            {
                playerMove.GiveSkill(i + 1);
                return;
            }
        }
    }

    private void SetUpMineObjs()
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

    private void ConfigureMineCell(GameObject cell, int value)
    {
        if (cell == null)
        {
            return;
        }

        if (cell.TryGetComponent(out MineArea mineArea))
        {
            mineArea.var = value;
        }

        if (cell.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = minedef;
        }
    }

    private IEnumerator FinishGambleAfterDelay(GameObject gambleObject)
    {
        yield return new WaitForSeconds(gambleFinishTime);

        if (gambleObject != null)
        {
            gambleObject.SetActive(false);
        }

        GameManager.Instance?.EndGamble();
    }

    private void ResolvePlayerReferences()
    {
        if (playerMove != null && playerHp != null)
        {
            return;
        }

        PlayerMarker player = FindAnyObjectByType<PlayerMarker>();
        if (player == null)
        {
            return;
        }

        playerMove = player.GetComponent<PlayerMovement>();
        playerHp = player.GetComponent<PlayerHealth>();
    }

    private void CacheCoinReferences()
    {
        if (coinF == null)
        {
            return;
        }

        coinAnim ??= coinF.GetComponent<Animator>();
        coinRenderer ??= coinF.GetComponent<SpriteRenderer>();
    }

    private Sprite GetRandomSlotSprite()
    {
        return slotSprites[Random.Range(0, slotSprites.Length)];
    }

    private Sprite GetSlotSprite(int index)
    {
        if (slotObjes == null || slotObjes.Length <= index || slotObjes[index] == null)
        {
            return null;
        }

        return slotObjes[index].TryGetComponent(out SpriteRenderer spriteRenderer) ? spriteRenderer.sprite : null;
    }

    private void SetSlotSprite(int index, Sprite sprite)
    {
        if (slotObjes != null && slotObjes.Length > index && slotObjes[index] != null &&
            slotObjes[index].TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private static void PlayAudio(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null)
        {
            return;
        }

        source.clip = clip;
        source.Play();
    }
}
