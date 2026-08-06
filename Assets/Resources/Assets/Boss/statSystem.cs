using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class statSystem : MonoBehaviour
{
    Rigidbody2D rb;
    bossSkills bs;

    public float currentHp, maxHp;
    private float prevHp;
    public int handler = 0; 

    public bool isPlayerAlive;
    public bool canGetDamage;

    private float timeSinceLastHit = 0f;

    float percent;
    public Image healthbar;

    bool a, s, d, f, g, h, j, k, l;


    void Start()
    {
        currentHp = maxHp;
        prevHp = currentHp;
        isPlayerAlive = true;
        canGetDamage = true;

        rb = GetComponent<Rigidbody2D>();
        bs = GetComponent<bossSkills>();
    }

    private void Update()
    {
        isHpChange();
        percent = currentHp * 100 / maxHp;
        float v = 10 * (percent) / 100;
        Vector3 targetScale = new Vector3(v, 0.4f, 1f);
        healthbar.rectTransform.localScale = targetScale;
    }

    void isHpChange()
    {

        if (prevHp != currentHp)
        {
            isPlayerDead();

                    
        }
    }
    void chechkSkill()
    {
        if (80 < currentHp && currentHp < 90 && !a) { bs.CoinFlip(); a = true; }

        if (70 < currentHp && currentHp < 80 && !s) { bs.SpinSlot(); s = true; }

        if (60 < currentHp && currentHp < 70 && !d) { bs.PlayMineFarm(); d = true; }

        if (50 < currentHp && currentHp < 60 && !f) { bs.CoinFlip(); f = true; }

        if (40 < currentHp && currentHp < 50 && !g) { bs.SpinSlot(); g = true; }

        if (30 < currentHp && currentHp < 40 && !h) { bs.PlayMineFarm(); h = true; }

        if (20 < currentHp && currentHp < 30 && !j) { bs.cheat = true; bs.CoinFlip(); j = true; }

        if (10 < currentHp && currentHp < 20 && !k) { bs.SpinSlot(); k = true; }

        if (0 < currentHp && currentHp < 10 && !l) { bs.PlayMineFarm(); l = true; }
    }

    public void isPlayerDead()
    {
        if (currentHp <= 0)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        isPlayerAlive = false;
        Debug.Log("Player died!");
        this.enabled = false;
    }

    public void GetDamage(float hasarMiktari)
    {
        if (canGetDamage)
        {
            canGetDamage = false;
            prevHp = currentHp;
            currentHp -= hasarMiktari;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
            CameraShake.Instance.Shake(0.3f, 0.5f);
            chechkSkill();

            isPlayerDead();

            timeSinceLastHit = Time.time - timeSinceLastHit;
            StartCoroutine(DamageCD());
        }
    }


    private IEnumerator DamageCD()
    {
        yield return new WaitForSeconds(0.5f);
        canGetDamage = true;
    }
}
