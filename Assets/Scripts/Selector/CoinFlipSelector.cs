using UnityEngine;
using System.Collections;

public class CoinFlipSelector : MonoBehaviour
{
    public int a = 0;
    public GameObject parent;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private Gambles gambles;

    private Coroutine timer;

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
        gambles.selectedSide = a;
        gambles.CoinFlip();
        parent.SetActive(false);
    }

    public void SetSelector(){
        parent.SetActive(true);
    }
}
