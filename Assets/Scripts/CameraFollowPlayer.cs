using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Vector3 differenceToPlayer;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.Slerp(this.transform.position, playerTransform.position + differenceToPlayer, 5.0f * Time.deltaTime);
    }

    public void SetPlayerToFollow(Transform transform)
    {
        playerTransform = transform;
    }
}
