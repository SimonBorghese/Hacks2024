using UnityEngine;
using UnityEngine.InputSystem;

public class HandPickerup : MonoBehaviour
{
    public Rigidbody CurrentRigidBody;

    public InputActionReference HandPickup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CurrentRigidBody != null)
        {
            float Axis = HandPickup.action.ReadValue<float>();

            if (Axis > 0.1f)
            {
                CurrentRigidBody.linearVelocity = (transform.position - CurrentRigidBody.position) * 10.0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            CurrentRigidBody = other.attachedRigidbody;
        }
    }
}
