using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Camera playerCamera;

    public float speed;

    public bool SuperSense;

    public Volume PostProcessingVolume;

    public PickupObject CurrentObjectTarget;

    public TMP_Text RenderText;

    public float FadeInTime;

    public float CurrentFadeTime;

    public float CurrentTarget;

    public bool FadeIn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Update texture transparency
        if (FadeIn)
        {
            RenderText.alpha = (CurrentFadeTime / FadeInTime);
            CurrentFadeTime += Time.deltaTime;

            if (CurrentFadeTime >= FadeInTime)
            {
                
            }
        }
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

        if (Input.GetKey(KeyCode.Q))
        {
            SuperSense = true;
        }
        else
        {
            SuperSense = false;
        }

        InputDirection.y = 0;

        // Apply the movement
        controller.Move((InputDirection + GravityDirection) * (speed * Time.deltaTime));
        
        // Apply post processing
        if (SuperSense && PostProcessingVolume.weight < 1.0)
        {
            PostProcessingVolume.weight += Time.deltaTime;
        }
        else if (!SuperSense && PostProcessingVolume.weight > 0.0)
        {
            PostProcessingVolume.weight -= Time.deltaTime;
        }
    }
}
