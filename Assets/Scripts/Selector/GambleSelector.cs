using System.Collections;
using UnityEngine;

public class GambleSelector : MonoBehaviour
{
    public int a = 0;
    GameObject parent;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private CoinFlipSelector cfs;

    private Coroutine timer;
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = StartCoroutine(WaitAndCall());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
    }

    IEnumerator WaitAndCall()
    {
        yield return new WaitForSeconds(waitTime);

        OnSelect();

        timer = null;
    }
    void OnSelect()
    {
        if (a == 1)
        {
            cfs.gameObject.SetActive(true);
            cfs.SetSelector();
            parent.SetActive(false);
        }
        else
        {
            GameManager.Instance.SpawnGamble(a);
            parent.SetActive(false);
        }

    }
}
