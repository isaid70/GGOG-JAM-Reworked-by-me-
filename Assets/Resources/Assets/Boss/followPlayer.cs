using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 0f, -5f);
    private Vector3 smoothVector = Vector3.zero;
    public bool Lerp;
    Vector3 targetPosition;
    public float camLag = 5f;
    void Start()
    {
    }

    void Update()
    {
         targetPosition = playerTransform.position + offset;

    }

    private void LateUpdate()
    {
        if (Lerp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVector, 0.3f);

            //camLag * Time.deltaTime camera lagi istersek 1f'in yerine yaz
        }
        else
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, offset.z);
        }
    }


}
