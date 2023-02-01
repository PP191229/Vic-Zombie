using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public float speed;
    public float sensX;
    public float sensY;
    public float force;
    float xRotation;
    float yRotation;
    public Camera playerCamera;
    [SerializeField] private float sensitivity;
    [SerializeField] Transform bulletPrefab;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] Rigidbody bulletRB;




    private NetworkVariable<int> maxAmmo = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> currentAmmo = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        maxAmmo.Value = 10;
        currentAmmo.Value = maxAmmo.Value;
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo.Value > 0)
        {
            Shooting(bulletSpawner);
            currentAmmo.Value--;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo.Value = maxAmmo.Value;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Movement();
        MoveCamera();
    }

    private void Movement() 
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        transform.position += moveDirection * speed * Time.deltaTime;
        /*
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        transform.position += playerMovementInput * speed * Time.deltaTime;*/
    }

    private void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Shooting(Transform firepoint)
    {
        var projectileObj = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);
        bulletRB = projectileObj.GetComponent<Rigidbody>();
        bulletRB.isKinematic = false;
        projectileObj.GetComponent<NetworkObject>().Spawn(true);
        Vector3 direction = transform.forward * force;
        bulletRB.AddForce(direction, ForceMode.Impulse);
    }
}
