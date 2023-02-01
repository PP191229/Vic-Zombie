using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkBullet : NetworkBehaviour
{
    public float force;
    private Rigidbody rB;
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Awake()
    {
        rB = GetComponent<Rigidbody>();
        rB.isKinematic = false;
        Vector3 direction = transform.forward * force;
        rB.AddForce(direction, ForceMode.Impulse);
    }
}
