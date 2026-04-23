using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f); //Colle la caméra sur la position du joueur
    }
}
