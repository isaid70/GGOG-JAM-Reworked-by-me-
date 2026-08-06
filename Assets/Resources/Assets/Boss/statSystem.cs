using UnityEngine;
using UnityEngine.UI;

public class statSystem : MonoBehaviour
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
    BossAttacks attacks;
    private bool enteredPhase2;
    float percent = 100;
    private void Awake()
    {
        attacks = GetComponent<BossAttacks>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    private void Update()
    {
        
        if (!enteredPhase2 && percent <= 50)
        {
            enteredPhase2 = true;
            attacks.EnterPhase2();
        }
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        percent = currentHealth / maxHealth * 100f;

        UpdateHealthBar();
        CheckGambleState();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        float v = 10 * (percent) / 100;
        Vector3 targetScale = new Vector3(v, 0.4f, 1f);
        healthBar.rectTransform.localScale = targetScale;
    }

    void CheckGambleState()
    {
        

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
            return;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}