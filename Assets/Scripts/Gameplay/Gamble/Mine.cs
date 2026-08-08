using UnityEngine;
using System.Collections;   

public class Mine : MonoBehaviour
{
    public int win = 0;
    public BossHealth boss;


    public void CheckWin()
    {
        if (win == 8)
        {
            WinMine();
        }
    }
    void WinMine()
    {
        if (boss != null)
        {
            boss.GetDamage(100);
        }

        StartCoroutine(durat());
    }
    IEnumerator durat()
    {
        yield return new WaitForSeconds(2f);
        win = 0;
        gameObject.SetActive(false);
        GameManager.Instance?.EndGamble();
    }
}

