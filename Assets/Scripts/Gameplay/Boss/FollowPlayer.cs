using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 0f, -5f);
    public bool Lerp;
    public float camLag = 5f;

    private Vector3 smoothVector;
    private Vector3 targetPosition;

    private void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        targetPosition = playerTransform.position + offset;
    }

    private void LateUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }

        if (Lerp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVector, 0.3f);
        }
        else
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, offset.z);
        }
    }
}
