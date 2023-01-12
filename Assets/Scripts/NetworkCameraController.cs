using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class NetworkCameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public override void OnNetworkSpawn()
    {
        if (IsOwner) cameraHolder.SetActive(true);
    }

}
