using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoSlotMachine : MonoBehaviour
{
    [Header("Slot Çarklarý (1, 2, 3)")]
    public SlotReel reel1;
    public SlotReel reel2;
    public SlotReel reel3;

    [Header("Sembol Havuzu")]
    public List<Sprite> availableSymbols;

    [Header("Hileli Oran Ayarlarý (%)")]
    [Range(0, 100)] public int nearMissChance = 50;
    [Range(0, 100)] public int winChance = 10;//ikisinin toplamýný 100den çýkarýnca tam kayýp

    [SerializeField] private float gambleFinishTime = 2f;
    [SerializeField] private PlayerMovement player;

    private bool isSpinning = false;

    public void OnSpinButtonClicked()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    public IEnumerator SpinRoutine()
    {
        isSpinning = true;

        // ilk hesap ondan sonra gösteri
        Sprite symbol1, symbol2, symbol3;
        CalculateScriptedOutcome(out symbol1, out symbol2, out symbol3);

        // dönmeye baþlama
        reel1.StartSpinning();
        reel2.StartSpinning();
        reel3.StartSpinning();

        // çarklarý sýrayla durduruyoruz moruq hileli þekilde.

        yield return new WaitForSeconds(1.5f);
        reel1.StopSpin(symbol1);

        yield return new WaitForSeconds(0.8f);
        reel3.StopSpin(symbol3); 

        // son sembol durmadan önce fazla bekletiyoz puahha
        yield return new WaitForSeconds(1.2f);
        reel2.StopSpin(symbol2);

        //son
        yield return new WaitForSeconds(0.5f);
        if (symbol1 == symbol2 && symbol2 == symbol3)
        {
            Debug.Log("win" + symbol1.name);//sembol namesine göre skill verilebilir bilmiyorum
            player?.GiveSkill(Random.Range(1, 2)); // 2 yerine skill sayýsýný yazacan ileride þimdilik 1 skill var diye 2.
            // win sit
        }
        else
        {
            Debug.Log("lose");
        }

        isSpinning = false;
        StartCoroutine(FinishGambleAfterDelay(this.gameObject));
    }

    private void CalculateScriptedOutcome(out Sprite s1, out Sprite s2, out Sprite s3)
    {
        int roll = Random.Range(0, 100);

        if (roll < winChance)
        {
            //kazanma
            Sprite target = GetRandomSymbol();
            s1 = target;
            s2 = target;
            s3 = target;

        }
        else if (roll < (winChance + nearMissChance))
        {
            //sol sað ayný orta farklý
            s1 = GetRandomSymbol();
            s3 = s1; // 1 ve 3 ayný yapýldý

            // farklý olana kadar while
            do
            {
                s2 = GetRandomSymbol();
            } while (s2 == s1);
        }
        else
        {
            //3 farklý
            s1 = GetRandomSymbol();

            do
            {
                s2 = GetRandomSymbol();
            } while (s2 == s1);

            do
            {
                s3 = GetRandomSymbol();
            } while (s3 == s1 || s3 == s2);
        }

    }

    private Sprite GetRandomSymbol()
    {
        return availableSymbols[Random.Range(0, availableSymbols.Count)];
    }

    private IEnumerator FinishGambleAfterDelay(GameObject gambleObject)
    {
        yield return new WaitForSeconds(gambleFinishTime);

        if (gambleObject != null)
        {
            gambleObject.SetActive(false);
        }

        GameManager.Instance?.EndGamble();
    }
}