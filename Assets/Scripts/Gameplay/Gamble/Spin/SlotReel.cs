using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer symbolDisplay;
    [SerializeField] private List<Sprite> spinAnimationSprites;

    [Header("Akýþ Ayarlarý")]
    [SerializeField] private float rollSpeed = 15f;
    [SerializeField] private float resetYPosition = 1.5f;

    [Header("Bounce (Esnetme) Ayarlarý")]
    [SerializeField] private float bounceDistance = 0.2f;
    [SerializeField] private float bounceDuration = 0.12f;

    private bool isSpinning = false;
    private Vector3 startLocalPos;

    private void Awake()
    {
        if (symbolDisplay != null)
        {
            startLocalPos = symbolDisplay.transform.localPosition;
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
        StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        int spriteIndex = 0;

        while (isSpinning)
        {
            // aþaðý
            symbolDisplay.transform.localPosition += Vector3.down * rollSpeed * Time.deltaTime;

            // biraz inince yeni prite + yukarý
            if (symbolDisplay.transform.localPosition.y <= startLocalPos.y - 1f)
            {

                Vector3 newPos = symbolDisplay.transform.localPosition;
                newPos.y += 1f;
                symbolDisplay.transform.localPosition = newPos;


                symbolDisplay.sprite = spinAnimationSprites[spriteIndex];
                spriteIndex = (spriteIndex + 1) % spinAnimationSprites.Count;
            }

            yield return null;
        }
    }

    public void StopSpin(Sprite finalSymbol)
    {
        isSpinning = false;
        StopAllCoroutines();
        StartCoroutine(SnapAndBounceRoutine(finalSymbol));
    }

    private IEnumerator SnapAndBounceRoutine(Sprite finalSymbol)
    {
        //final
        symbolDisplay.sprite = finalSymbol;

        // bitiminde yukarý kaydýrt
        Vector3 topPos = startLocalPos + new Vector3(0, 0.8f, 0);
        symbolDisplay.transform.localPosition = topPos;

        // merkez
        float elapsed = 0f;
        float snapDuration = 0.1f;

        while (elapsed < snapDuration)
        {
            symbolDisplay.transform.localPosition = Vector3.Lerp(topPos, startLocalPos, elapsed / snapDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        //sekme
        Vector3 downPos = startLocalPos + new Vector3(0, -bounceDistance, 0);

        // aþaðý
        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            symbolDisplay.transform.localPosition = Vector3.Lerp(startLocalPos, downPos, elapsed / bounceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // yukarý
        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            float t = elapsed / bounceDuration;
            t = t * t * (3f - 2f * t);
            symbolDisplay.transform.localPosition = Vector3.Lerp(downPos, startLocalPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        symbolDisplay.transform.localPosition = startLocalPos;
    }
}