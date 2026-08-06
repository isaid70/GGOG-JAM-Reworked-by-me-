using System.Collections;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{



    public AudioSource sourceslot;
    public AudioClip clipslot;


    public GameObject[] dices;
    public Transform player;
    public float travelTime = 1f;
    public float arcHeight = 2f;
    public float accelerationCurve = 2f;
    public float returnTime = 1f;

    public float attackCd;


    private Vector3 originalPosition;

    public bool inFight;
    public bool canCast;

    Flux flux;
    RainSpell spell;

    int a = 4;

    /*private void Start()
    {
        ThrowDice();
    }*/

    private void Start()
    {
        flux = GetComponentInChildren<Flux>();
        spell = GetComponent<RainSpell>();
    }
    private void Awake()
    {
        StartCoroutine(canCst());
    }
    IEnumerator canCst()
    {
        yield return new WaitForSeconds(1.5f);
        canCast = true;
    }


    private void Update()
    {
        if (inFight && canCast)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        canCast = false;
        if (a %3 == 1)
        {
            ThrowDice();


            sourceslot.clip = clipslot;
            sourceslot.Play();

            a++;
        }
        else if (a%3 == 2)
        {
            spell.CastSpell();
            a++;
        }
        else if (a % 3 == 0)
        {
            flux.CastSpell();
            a++;
        }
        yield return new WaitForSeconds(attackCd);
        canCast = true;
    }

    public void ThrowDice()
    {
        originalPosition = dices[0].transform.position;
        Transform dice = dices[0].transform;
        StartCoroutine(MoveDiceAlongCurve(dice));
    }

    private IEnumerator MoveDiceAlongCurve(Transform dice)
    {
        Vector3 startPos = dice.position;
        Vector3 endPos = player.position;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            if (dice.position.y - player.position.y < 0)
            {

                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(Mathf.Pow(elapsed / travelTime, accelerationCurve));

                Vector3 linearPos = Vector3.Lerp(startPos, endPos, t);

                float heightOffset = arcHeight * Mathf.Sin(Mathf.PI * t);

                dice.position = new Vector3(linearPos.x + heightOffset, linearPos.y, linearPos.z);

                yield return null;
            }
            else
            {

                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(Mathf.Pow(elapsed / travelTime, accelerationCurve));

                Vector3 linearPos = Vector3.Lerp(startPos, endPos, t);

                float heightOffset = arcHeight * Mathf.Sin(Mathf.PI * t);

                dice.position = new Vector3(linearPos.x, linearPos.y + heightOffset, linearPos.z);

                yield return null;
            }
        }

        CameraShake.Instance.Shake(0.3f, 0.2f);
        ReturnDice();
    }
    public void ReturnDice()
    {
        if (dices.Length == 0) return;

        Transform dice = dices[0].transform;
        StartCoroutine(MoveDiceBack(dice));
    }

    private IEnumerator MoveDiceBack(Transform dice)
    {
        Vector3 startPos = dice.position;
        Vector3 endPos = originalPosition;
        float elapsed = 0f;

        while (elapsed < returnTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(Mathf.Pow(elapsed / returnTime, accelerationCurve));

            // D�md�z, kavis yok
            dice.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Hareket bitince tam yerine oturt
        dice.position = endPos;
    }
}