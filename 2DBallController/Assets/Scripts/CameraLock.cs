using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour
{
    public Transform player = null;

    void Update()
    {

        transform.position = new Vector3 (player.position.x, player.position.y, transform.position.z);

    }

}
