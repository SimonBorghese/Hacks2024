using UnityEngine;

public class SteelDoor : MonoBehaviour
{

    public Material alternativeMaterial;

    public string ExchangeKeycardName;

    public bool Active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            GetComponent<MeshRenderer>().material = alternativeMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard"))
        {
            Keycard card = other.GetComponent<Keycard>();
            if (card.Name.Equals(ExchangeKeycardName))
            {
                GetComponent<MeshCollider>().enabled = false;
                GetComponent<MeshRenderer>().material = alternativeMaterial;
                Destroy(other.gameObject);
                Active = true;
            }
        }
    }
}
