using System.Collections;
using UnityEngine;

public class MineArea : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public BossHealth boss;

    public float openTime = 2f;
    public Sprite[] mineSprites;
    public int var;
    public GameObject Mine;

    private float timeInsideArea;
    private bool playerInside;
    private Mine mineWin;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mineWin = Mine != null ? Mine.GetComponent<Mine>() : null;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMarker _))
        {
            playerInside = true;
            timeInsideArea = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMarker _))
        {
            playerInside = false;
            timeInsideArea = 0f;
        }
    }

    private void Update()
    {
        if (!playerInside)
        {
            return;
        }

        timeInsideArea += Time.deltaTime;
        if (timeInsideArea > openTime)
        {
            OpenArea();
            playerInside = false;
        }
    }

    private void OpenArea()
    {
        if (var == 0)
        {
            SetMineSprite(0);
            PlayMineSound();
            StartCoroutine(CloseAfterDelay());
        }
        else if (var == 1 && mineWin != null)
        {
            mineWin.win++;
            mineWin.CheckWin();
            SetMineSprite(1);
        }
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (mineWin != null)
        {
            mineWin.win = 0;
        }

        if (Mine != null)
        {
            Mine.SetActive(false);
        }

        GameManager.Instance?.EndGamble();
    }

    private void SetMineSprite(int index)
    {
        if (spriteRenderer != null && mineSprites != null && mineSprites.Length > index)
        {
            spriteRenderer.sprite = mineSprites[index];
        }
    }

    private void PlayMineSound()
    {
        if (source == null || clip == null)
        {
            return;
        }

        source.clip = clip;
        source.Play();
    }
}
