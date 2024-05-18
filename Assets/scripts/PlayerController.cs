using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Camera playerCamera;

    public float speed;

    public bool SuperSense;

    public Volume PostProcessingVolume;

    public PickupObject CurrentObjectTarget;

    public GameObject TargetObject;

    public TMP_Text RenderText;

    public float FadeInTime;

    public float CurrentFadeTime;

    public float CurrentTarget;

    public bool FadeIn;

    public Rigidbody heldBody;

    public float forwardHold;

    public Image DetectionImage;

    public float MaxDetection;

    public float CurrentDetection;

    public Image Blackout;

    public GameObject ScanPanel;

    public TMP_Text ItemText;

    public TMP_Text DescriptionText;

    public string DefaultItemText;

    public string DefaultDescriptionText;
// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        RenderText.alpha = 0.0f;

        DefaultItemText = ItemText.text;
        DefaultDescriptionText = DescriptionText.text;
        ScanPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SuperSense)
        {
            ItemText.text = "";
            DescriptionText.text = "";
            ScanPanel.SetActive(true);
            RaycastHit[] HitDetects = Physics.RaycastAll(transform.position, playerCamera.transform.forward, 100.0f);

            foreach (var H in HitDetects)
            {
                if (H.collider.CompareTag("Scannable"))
                {
                    var ScanInfo = H.collider.GetComponent<ScannablePrefabs>().ObjectInfo;

                    ItemText.text = DefaultItemText + ScanInfo.ItemName;
                    DescriptionText.text = DefaultDescriptionText + ScanInfo.ItemDescription;
                }
            }
        }
        else
        {
            ScanPanel.SetActive(false);
        }
        DetectionImage.transform.localScale = new Vector3(3.0f * (CurrentDetection / MaxDetection), 0.45667f, 1.0f);
        Blackout.color = new Color(0.0f, 0.0f, 0.0f, (CurrentDetection / MaxDetection));

        if (CurrentDetection >= MaxDetection)
        {
            SceneManager.LoadScene(0);
        }
        if (heldBody != null)
        {
            heldBody.linearVelocity = ((transform.position + (transform.forward * forwardHold)) - heldBody.position) * 20.0f;
        }

        // Update texture transparency
        if (FadeIn)
        {
            RenderText.alpha = (CurrentFadeTime / FadeInTime);
            CurrentFadeTime += Time.deltaTime;

            if (CurrentFadeTime >= FadeInTime)
            {
                FadeIn = false;
            }
        }
        

        if (!FadeIn && CurrentFadeTime > 0.0f)
        {
            RenderText.alpha = (CurrentFadeTime / FadeInTime);
            CurrentFadeTime -= Time.deltaTime;
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
            ScanPanel.SetActive(true);
        }
        else
        {
            SuperSense = false;
            ScanPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObjectTarget != null)
            {
                FadeIn = true;
                RenderText.text = CurrentObjectTarget.Notice;
                Destroy(TargetObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldBody == null){
                
                RaycastHit[] Hit = Physics.RaycastAll(transform.position, transform.forward, 100.0f);

                foreach (var H in Hit)
                {
                    if (H.rigidbody)
                    {
                        heldBody = H.rigidbody;
                    }
                }
            }
            else
            {
                heldBody = null;
            }
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
