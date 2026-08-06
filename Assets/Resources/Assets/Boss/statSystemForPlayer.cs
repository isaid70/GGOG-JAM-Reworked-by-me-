using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class statSystemForPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    bossSkills bs;

    public float currentHp, maxHp;
    float percent;
    private float prevHp;
    public int handler = 0;

    public bool isPlayerAlive;
    public bool canGetDamage;

    private float timeSinceLastHit = 0f;

    private SpriteRenderer spriteRenderer;
    Material material;

    public Image healtBar;

    void Start()
    {
        currentHp = maxHp;
        prevHp = currentHp;
        isPlayerAlive = true;
        canGetDamage = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        rb = GetComponent<Rigidbody2D>();
        bs = GetComponent<bossSkills>();
        Flash();
    }

    private void Update()
    {
        isHpChange();

        percent = currentHp * 100 / maxHp;
        float v = 10 * (percent) / 100;
        Vector3 targetScale = new Vector3(v, 0.4f, 1f);
        healtBar.rectTransform.localScale = targetScale;
    }

    void isHpChange()
    {

        if (prevHp != currentHp)
        {
            isPlayerDead();
        }
    }

    public void isPlayerDead()
    {
        if (currentHp <= 0)
        {
            KillPlayer();
            UIManager.Instance.ActivateDeadPanel();
            Time.timeScale = 0;
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
            Flash();
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


    private void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        material.SetFloat("_FlashAmount", 1); // Özel shader kullanýyorsan
        yield return new WaitForSeconds(0.1f);
        material.SetFloat("_FlashAmount", 0);
    }
}
