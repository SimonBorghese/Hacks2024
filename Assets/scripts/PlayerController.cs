using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Camera playerCamera;

    public float speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMovement = new Vector3();
        cameraMovement.x = -Input.GetAxis("Mouse Y");
        cameraMovement.y = Input.GetAxis("Mouse X");
        transform.Rotate(cameraMovement);

        Vector3 cameraRotation = transform.eulerAngles;
        cameraRotation.z = 0.0f;
        transform.eulerAngles = cameraRotation;
        
        Vector3 InputDirection = new Vector3();
        Vector3 GravityDirection = new Vector3(0.0f, -9.8f, 0.0f);
        if (Input.GetKey(KeyCode.W))
        {
            InputDirection += playerCamera.transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            InputDirection -= playerCamera.transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            InputDirection -= playerCamera.transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            InputDirection += playerCamera.transform.right;
        }

        InputDirection.y = 0;

        // Apply the movement
        controller.Move((InputDirection + GravityDirection) * (speed * Time.deltaTime));
    }
}
