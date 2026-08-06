using UnityEngine;

public class GiveDamageToPlayer : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<statSystemForPlayer>() != null)
        {
            collision.GetComponent<statSystemForPlayer>().GetDamage(damage);
        }
    }
}
