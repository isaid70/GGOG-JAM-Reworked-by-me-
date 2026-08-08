using UnityEngine;
using System.Collections;   

public class Mine : MonoBehaviour
{
    public int win = 0;
    public statSystem boss;


    public void ChechkWin()
    {
        if (win == 8)
        {
            WinMine();
        }
    }
    void WinMine()
    {
        boss.GetDamage(100);
        StartCoroutine(durat());
    }
    IEnumerator durat()
    {
        yield return new WaitForSeconds(2f);
        win = 0;
        this.gameObject.SetActive(false);
        GameManager.Instance.EndGamble();
    }
}
