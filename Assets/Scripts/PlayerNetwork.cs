using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private Vector3 playerMovementInput;
    public float speed;
    private Vector2 playerMouseInput;
    private float xRot;
    public Camera playerCamera;
    [SerializeField] private float sensitivity;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Movement();
        MoveCamera();
    }

    private void Movement() 
    {
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        transform.position += playerMovementInput * speed * Time.deltaTime;
    }

    private void MoveCamera()
    {
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        xRot -= playerMouseInput.y * sensitivity;

        transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);
        xRot = Mathf.Clamp(xRot, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);


    }
}
