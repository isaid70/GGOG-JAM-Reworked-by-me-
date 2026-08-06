using System.Collections;
using UnityEngine;

public class MineAreas : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

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
        if (playerInside)
        {
            timeInsideArea += Time.deltaTime;
            if (timeInsideArea > openTime)
            {
                OpenArea();
                playerInside = false;
            }
            if (a == 8) { StartCoroutine(durat()); }
        }
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
        yield return new WaitForSeconds(1f);
        Mine.SetActive(false);
    }
}
