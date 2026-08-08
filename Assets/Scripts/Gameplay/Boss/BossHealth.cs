using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Health Bar")]
    [SerializeField] private Image healthBar;

    [Header("Gamble Thresholds (%)")]
    [SerializeField] private float firstThreshold = 75f;
    [SerializeField] private float secondThreshold = 50f;
    [SerializeField] private float thirdThreshold = 25f;

    private bool firstTriggered;
    private bool secondTriggered;
    private bool thirdTriggered;
    private bool enteredPhase2;
    private BossAttacks attacks;

    private void Awake()
    {
        attacks = GetComponent<BossAttacks>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        float percent = HealthPercent;
        if (!enteredPhase2 && percent <= 50f)
        {
            enteredPhase2 = true;
            attacks?.EnterPhase2();
        }
    }

    public void GetDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        UpdateHealthBar();
        CheckGambleState();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null || maxHealth <= 0)
        {
            return;
        }

        healthBar.rectTransform.localScale = new Vector3(10f * (HealthPercent / 100f), 0.4f, 1f);
    }

    private void CheckGambleState()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        float percent = HealthPercent;

        if (!firstTriggered && percent <= firstThreshold)
        {
            firstTriggered = true;
            GameManager.Instance.StartGamble();
            return;
        }

        if (!secondTriggered && percent <= secondThreshold)
        {
            secondTriggered = true;
            GameManager.Instance.StartGamble();
            return;
        }

        if (!thirdTriggered && percent <= thirdThreshold)
        {
            thirdTriggered = true;
            GameManager.Instance.StartGamble();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private float HealthPercent => maxHealth <= 0 ? 0 : currentHealth / maxHealth * 100f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
