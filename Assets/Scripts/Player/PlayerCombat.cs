using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1.2f;
    public float attackRadius = 0.6f;
    public int damage = 20;

    public LayerMask enemyLayer;

    private Animator anim;

    [Header("Orbit")]
    public GameObject orbitObject;
    public float rotateSpeed = 120;
    public float duration = 5;

    [Header("Swing")]
    public GameObject swingObject;
    Vector2 direction;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        /*if (Input.mousePosition.y > Screen.height / 1.7)
        {
            anim.Play("Attack_Up");
        }
        else
        {
            anim.Play("Attack_Side");
        }*/

        direction = GetMouseDirection();
        swingObject.SetActive(true);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("Angle: " + angle);
        if (angle + 180 < 270 && angle + 180 > 90)
        {
            //swingObject.GetComponent<SpriteRenderer>().flipY = false;
            swingObject.transform.rotation = Quaternion.Euler(0, 0, angle - 20);
        }
        else { //swingObject.GetComponent<SpriteRenderer>().flipY = true;
               swingObject.transform.rotation = Quaternion.Euler(180, 0, -angle- 20); }



    }

    // Animation Event
    public void DealDamage()
    {
        direction = GetMouseDirection();

        Vector2 attackCenter =
            (Vector2)transform.position + direction * attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackCenter,
            attackRadius,
            enemyLayer);

        foreach (Collider2D hit in hits)
        {
            statSystem boss = hit.GetComponent<statSystem>();

            if (boss != null)
            {
                boss.GetDamage(damage);
            }
        }
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return (mouse - transform.position).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 direction = Vector2.right;

        if (Camera.main != null)
        {
            Vector3 mouse =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = (mouse - transform.position).normalized;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            (Vector2)transform.position + direction * attackRange,
            attackRadius);
    }

    public void StartOrbit()
    {
        StartCoroutine(OrbitBlade());
    }

    IEnumerator OrbitBlade()
    {
        orbitObject.SetActive(true);

        float timer = 0;

        while (timer < duration)
        {
            orbitObject.transform.Rotate(
                Vector3.forward,
                rotateSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        orbitObject.transform.localRotation = Quaternion.identity;

        orbitObject.SetActive(false);
    }
}