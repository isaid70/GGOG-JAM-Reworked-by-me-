using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RainSpell : MonoBehaviour
{

    public int _damage = 10;
    public float _cooldown = 1f;
    public bool _canCast = true;
    public int _magicCount = 5;

    public float _damageRange = 2f;
    public float _spawnRadius = 5f;
    public LayerMask _damageLayer = 0;

    public GameObject _magicPrefab;
    public Sprite[] Bullets;

    public Transform _enemyTarget;


    public void CastSpell()
    {

        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, _damageRange, _damageLayer);
        if (player != null)
        {
            if (_canCast)
            {
                for (int i = 0; i < _magicCount; i++)
                {
                    Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
                    Vector3 spawnPosition = _enemyTarget.position + new Vector3(randomOffset.x, randomOffset.y + 5f, 0);
                    int x = Random.Range(0, Bullets.Length);
                    GameObject magic = Instantiate(_magicPrefab, spawnPosition, Quaternion.identity);
                    magic.GetComponent<SpriteRenderer>().sprite = Bullets[x];
                    float abc = (spawnPosition - new Vector3(spawnPosition.x, -5f, spawnPosition.z)).magnitude;
                    magic.GetComponent<Rigidbody2D>().linearVelocity = -new Vector3(spawnPosition.x, abc, 0);
                    StartCoroutine(DestroyMagic(magic));

                }

                //StartCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Cooldown()
    {
        _canCast = false;
        yield return new WaitForSeconds(_cooldown);
        _canCast = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _damageRange);

        if (_enemyTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_enemyTarget.position, _spawnRadius);
        }
    }

    IEnumerator DestroyMagic(GameObject magic)
    {
        yield return new WaitForSeconds(2f);
        Destroy(magic);

    }
}
