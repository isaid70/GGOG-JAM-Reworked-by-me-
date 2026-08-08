using UnityEngine;

public class GiveDamageToEntity : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BossHealth bossHealth))
        {
            bossHealth.GetDamage(damage);
        }
    }
}

