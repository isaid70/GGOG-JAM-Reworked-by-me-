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

    public bool inFight;
    public bool canCast;

    private Vector3 originalPosition;
    private Flux flux;
    private RainSpell spell;
    [SerializeField] private int attackIndex = 4;

    private void Awake()
    {
        StartCoroutine(EnableCastingAfterDelay());
    }

    private void Start()
    {
        flux = GetComponentInChildren<Flux>();
        spell = GetComponent<RainSpell>();
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.CanBossAttack)
        {
            return;
        }

        if (inFight && canCast)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator EnableCastingAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        canCast = true;
    }

    public void EnterPhase2()
    {
        if (CurrentPhase == BossPhase.Phase2)
        {
            return;
        }

        CurrentPhase = BossPhase.Phase2;
        attackCd *= 0.7f;
    }

    private IEnumerator Attack()
    {
        canCast = false;

        if (CurrentPhase == BossPhase.Phase1)
        {
            Phase1Attack();
        }
        else
        {
            Phase2Attack();
        }

        yield return new WaitForSeconds(attackCd);
        canCast = true;
    }

    private void Phase1Attack()
    {
        int pattern = attackIndex % 3;

        if (pattern == 1)
        {
            ThrowDice();
            PlaySlotSound();
        }
        else if (pattern == 2)
        {
            spell?.CastSpell();
        }
        else
        {
            flux?.CastSpell();
        }

        attackIndex++;
    }

    private void Phase2Attack()
    {
        int pattern = attackIndex % 4;

        if (pattern == 1)
        {
            ThrowDice();
        }
        else if (pattern == 2)
        {
            spell?.CastSpell();
        }
        else if (pattern == 3)
        {
            flux?.CastSpell();
        }
        else
        {
            ThrowDice();
        }

        attackIndex++;
    }

    public void ThrowDice()
    {
        if (dices == null || dices.Length == 0 || dices[0] == null || player == null)
        {
            return;
        }

        originalPosition = dices[0].transform.position;
        StartCoroutine(MoveDiceAlongCurve(dices[0].transform));
    }

    private IEnumerator MoveDiceAlongCurve(Transform dice)
    {
        Vector3 startPos = dice.position;
        Vector3 endPos = player.position;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(Mathf.Pow(elapsed / travelTime, accelerationCurve));
            Vector3 linearPos = Vector3.Lerp(startPos, endPos, t);
            float heightOffset = arcHeight * Mathf.Sin(Mathf.PI * t);

            dice.position = dice.position.y - player.position.y < 0
                ? new Vector3(linearPos.x + heightOffset, linearPos.y, linearPos.z)
                : new Vector3(linearPos.x, linearPos.y + heightOffset, linearPos.z);

            yield return null;
        }

        CameraShake.Instance?.Shake(0.3f, 0.2f);
        ReturnDice();
    }

    public void ReturnDice()
    {
        if (dices == null || dices.Length == 0 || dices[0] == null)
        {
            return;
        }

        StartCoroutine(MoveDiceBack(dices[0].transform));
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
            dice.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        dice.position = endPos;
    }

    public void StopAttacks()
    {
        StopAllCoroutines();
        canCast = false;

        if (dices != null && dices.Length > 0 && dices[0] != null)
        {
            dices[0].transform.localPosition = new Vector3(-0.35f, 0.35f, 0);
        }
    }

    private void PlaySlotSound()
    {
        if (sourceslot == null || clipslot == null)
        {
            return;
        }

        sourceslot.clip = clipslot;
        sourceslot.Play();
    }
}
