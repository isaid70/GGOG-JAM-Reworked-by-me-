using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer symbolDisplay;
    [SerializeField] private List<Sprite> spinAnimationSprites;
    [SerializeField] private float spinSpeed = 0.05f;

    private bool isSpinning = false;

    public void StartSpinning()
    {
        isSpinning = true;
        StartCoroutine(AnimateSpin());
    }

    private IEnumerator AnimateSpin()
    {
        int index = 0;
        while (isSpinning)
        {
            symbolDisplay.sprite = spinAnimationSprites[index];
            index = (index + 1) % spinAnimationSprites.Count;
            yield return new WaitForSeconds(spinSpeed);
        }
    }

    public void StopSpin(Sprite finalSymbol)
    {
        isSpinning = false;
        // anim durdu ve calculate edilmiþ sembol
        symbolDisplay.sprite = finalSymbol;
    }
}