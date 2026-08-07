using System.Collections;
using UnityEngine;

public class MineAreas : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public statSystem boss;

    private float timeInsideArea = 0f;
    public float openTime = 2f;
    private bool playerInside;
    public Sprite[] mineSprites;
    public int var;
    int a = 0;
    public GameObject Mine;

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerMain player = collision.gameObject.GetComponent<playerMain>();
        if (player != null)
        {
            playerInside = true;
            timeInsideArea = 0f;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        playerMain player = collision.gameObject.GetComponent<playerMain>();
        if (player != null)
        {
            playerInside = false;
            timeInsideArea = 0f;
        }
    }
    private void Update()
    {
        if (a == 8) { WinMine(); }

        if (playerInside)
        {
            timeInsideArea += Time.deltaTime;
            if (timeInsideArea > openTime)
            {
                OpenArea();
                playerInside = false;
            }
            
        }
    }
    void WinMine()
    {
        boss.GetDamage(100);
        StartCoroutine(durat());
    }
    void OpenArea()
    {
        if (var == 0)
        {
            GetComponent<SpriteRenderer>().sprite = mineSprites[0];

            source.clip = clip;

            //SoundFXManager.instance.PlaySoundFXClip(clip,transform,1f);
            source.Play();
            StartCoroutine(durat());
        }
        else if (var == 1)
        {
            a++;
            GetComponent<SpriteRenderer>().sprite = mineSprites[1];
        }
    }

    IEnumerator durat()
    {
        yield return new WaitForSeconds(2f);
        Mine.SetActive(false);
        GameManager.Instance.EndGamble();
    }
}
