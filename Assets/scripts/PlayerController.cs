using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    public InputActionReference JoystickL;

    public InputActionReference InteractButton;

    public InputActionReference SenseMode;

    public InputActionAsset Actions;

    public bool bDisregardInput;

    public float BaseCameraHeight;

    public int FoundEvidence;

    public int RequiredEvidence;
// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        RenderText.alpha = 0.0f;

        DefaultItemText = ItemText.text;
        DefaultDescriptionText = DescriptionText.text;
        ScanPanel.SetActive(false);

        BaseCameraHeight = controller.center.y;
    }

    private void OnEnable()
    {
        if (Actions != null)
        {
            Actions.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float BaseHeight = 1.361f;
        controller.height = BaseHeight + playerCamera.transform.localPosition.y;

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
        //Blackout.color = new Color(0.0f, 0.0f, 0.0f, (CurrentDetection / MaxDetection));

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
        Vector3 InputDirection = new Vector3();
        Vector3 GravityDirection = new Vector3(0.0f, -9.8f, 0.0f);
        Vector2 InputController = JoystickL.action.ReadValue<Vector2>();
        InputDirection += playerCamera.transform.forward * InputController.y;
 
        InputDirection += playerCamera.transform.right * InputController.x;
       
        float isSense = SenseMode.action.ReadValue<float>();
        float isInteract = InteractButton.action.ReadValue<float> ();


        if (isSense > 0.5f)
        {
            SuperSense = true;
            ScanPanel.SetActive(true);
        }
        else
        {
            SuperSense = false;
            ScanPanel.SetActive(false);
        }

        if (isInteract > 0.5f)
        {
            if (CurrentObjectTarget != null)
            {
                FadeIn = true;
                RenderText.text = CurrentObjectTarget.Notice;
                Destroy(TargetObject);
                CurrentObjectTarget = null;
                FoundEvidence++;
            }
        }

        if (FoundEvidence >= RequiredEvidence)
        {
            FadeIn = true;
            RenderText.text = "You have found enough evidence to prosecute, now get out before they catch you!";
        }

        InputDirection.y = 0;

        // Apply the movement
        controller.Move((InputDirection + GravityDirection) * (speed * Time.deltaTime));
        
        // Apply post processing
        if (SuperSense || PostProcessingVolume.weight < (CurrentDetection / MaxDetection))
        {
            PostProcessingVolume.weight += Time.deltaTime;
        }
        else if (!SuperSense || PostProcessingVolume.weight > (CurrentDetection / MaxDetection))
        {
            PostProcessingVolume.weight -= Time.deltaTime;
        }
    }
}
