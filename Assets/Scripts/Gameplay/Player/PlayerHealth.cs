using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHp;
    public float maxHp;
    public bool isPlayerAlive;
    public bool canGetDamage;

    public Image healthBar;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Start()
    {
        currentHp = maxHp;
        isPlayerAlive = true;
        canGetDamage = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer != null ? spriteRenderer.material : null;

        UpdateHealthBar();
        Flash();
    }

    public void GetDamage(float damageAmount)
    {
        if (!canGetDamage)
        {
            return;
        }

        canGetDamage = false;
        currentHp = Mathf.Clamp(currentHp - damageAmount, 0, maxHp);
        UpdateHealthBar();
        Flash();
        CheckPlayerDeath();
        StartCoroutine(DamageCooldown());
    }

    public void GetHeal(float healAmount)
    {
        currentHp = Mathf.Clamp(currentHp + healAmount, 0, maxHp);
        UpdateHealthBar();
        Flash();
    }

    private void CheckPlayerDeath()
    {
        if (currentHp > 0)
        {
            return;
        }

        KillPlayer();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActivateDeadPanel();
        }

        Time.timeScale = 0;
    }

    private void KillPlayer()
    {
        isPlayerAlive = false;
        enabled = false;
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canGetDamage = true;
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null || maxHp <= 0)
        {
            return;
        }

        float percent = currentHp / maxHp;
        healthBar.rectTransform.localScale = new Vector3(10f * percent, 0.4f, 1f);
    }

    private void Flash()
    {
        if (material != null && material.HasProperty("_FlashAmount"))
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.1f);
        material.SetFloat("_FlashAmount", 0);
    }
}
