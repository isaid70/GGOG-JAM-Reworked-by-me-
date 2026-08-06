using UnityEngine;

public class GiveDamageToEntity : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<statSystem>() != null)
        {
            collision.GetComponent<statSystem>().GetDamage(damage);
        }
    }
}
