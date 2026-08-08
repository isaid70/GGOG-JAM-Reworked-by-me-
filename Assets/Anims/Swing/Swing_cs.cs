using UnityEngine;

public class Swing_cs : MonoBehaviour
{
    public int attackDamage = 20;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossHealth stat = collision.GetComponent<BossHealth>();
        if (stat!=null)
        {
            stat.GetDamage(attackDamage);
        }
    }
}

