using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Flux : MonoBehaviour
{
    public AudioSource source; 
    public AudioClip clip;

    public int _damage = 10;
    public float _cooldown = 1f;
    public bool _canCast = true;
    public int _magicCount = 1;

    public float duration = 1.0f;


    public float _damageRange = 2f;
    public LayerMask _damageLayer = 0;

    public GameObject _magicPrefab;
    public Transform player;

    [SerializeField] private List<GameObject> fluxPool;


    public void CastSpell(Transform boss)
    {
        /*int magicToCast = 1 ;

            for (int i = 0; i < magicToCast; i++)
            {
                if (player.CompareTag("Player") && _canCast)
                {
                    
                    Vector3 spawnposition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Vector2 direction = player.transform.position - spawnposition;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    //
                    {
                        GameObject magic = Instantiate(_magicPrefab, spawnposition, Quaternion.identity);
                    magic.transform.parent = boss;
                    magic.transform.rotation = Quaternion.Euler(0, 0, angle + 150);

                    StartCoroutine(RotateToTarget(magic.transform.rotation.eulerAngles.z, magic.transform.rotation.eulerAngles.z - 90, magic));
                        //   magic.transform.localScale = new Vector3(1, scaleX, 1);

                    //SoundFXManager.instance.PlaySoundFXClip(clip,transform,1f);
                    }
                }
            }*/
        SpawnFlux(1, 180);

    }
    public void CastSpell2(Transform boss, int count)
    {
        StartCoroutine(SpawnFlux(count, 140));
    }

    IEnumerator SpawnFlux(int count, float angleOffSet) 
    {
        if(player != null && _canCast)
        {
            int a= 0;

            while (a < count) {
                Vector2 direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                GameObject magic = GetMagicFromPool();
                magic.SetActive(true);
                magic.transform.position = transform.position;
                magic.transform.rotation = Quaternion.Euler(0, 0, angle + angleOffSet);
                StartCoroutine(RotateToTarget(magic.transform.rotation.eulerAngles.z, magic.transform.rotation.eulerAngles.z - (angleOffSet-40), magic));
                yield return new WaitForSeconds(0.2f);
                a++;
            }

        }
    }
    private GameObject GetMagicFromPool()
    {
        foreach (var magic in fluxPool)
        {
            if (!magic.activeInHierarchy)
            {
                return magic;
            }
        }
        // Eğer havuzda kullanılabilir bir nesne yoksa, yeni bir tane oluşturabilirsiniz.
        GameObject newMagic = Instantiate(_magicPrefab);
        newMagic.SetActive(false); // Başlangıçta devre dışı bırak
        fluxPool.Add(newMagic);
        return newMagic;
    }


    IEnumerator RotateToTarget(float startAngle, float targetAngle, GameObject magic)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 0-1 aras� normalle�tir

            // Smoothstep uygularsak daha do�al olur:
            t = t * t * (3f - 2f * t);

            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, t);
            magic.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        // Tam hedef a��ya sabitle
        magic.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        magic.SetActive(false); // İşlem tamamlandıktan sonra nesneyi devre dışı bırak

    }


}
