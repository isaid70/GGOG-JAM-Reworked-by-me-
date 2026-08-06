using System.Collections;
using UnityEngine;

public enum BossPhase
{
    Phase1,
    Phase2
}


public class BossAttacks : MonoBehaviour
{

    public BossPhase CurrentPhase = BossPhase.Phase1;

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

    [SerializeField]int a = 4;

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
        if (!GameManager.Instance.CanBossAttack)
            return;

        if (inFight && canCast)
        {
            StartCoroutine(Attack());
        }
    }

    public void EnterPhase2()
    {
        CurrentPhase = BossPhase.Phase2;

        attackCd *= 0.7f;

        Debug.Log("PHASE 2");
    }



    IEnumerator Attack()
    {
        canCast = false;
        switch (CurrentPhase)
        {
            case BossPhase.Phase1:
                Phase1Attack();
                break;

            case BossPhase.Phase2:
                Phase2Attack();
                break;
        }
        yield return new WaitForSeconds(attackCd);
        canCast = true;

    }
    void Phase1Attack()
    {
        if (a % 3 == 1)
        {
            ThrowDice();

            sourceslot.clip = clipslot;
            sourceslot.Play();
        }
        else if (a % 3 == 2)
        {
            spell.CastSpell();
        }
        else
        {
            flux.CastSpell();
        }

        a++;
    }
    void Phase2Attack()
    {
        if (a % 4 == 1)
        {
            ThrowDice();
        }
        else if (a % 4 == 2)
        {
            spell.CastSpell();
        }
        else if (a % 4 == 3)
        {
            flux.CastSpell();
        }
        else
        {
            // Yeni saldırı
            Debug.Log("NEW ATTACK");
        }

        a++;
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
    public void StopAttacks()
    {
        StopAllCoroutines();

        canCast = false;


        dices[0].transform.localPosition = new Vector3(-0.35f, 0.35f, 0);
    }
}