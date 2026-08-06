using UnityEngine;

public class BossMain : MonoBehaviour
{
    [SerializeField]PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if(playerMovement.transform.position.x > transform.position.x)
        {
            
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
