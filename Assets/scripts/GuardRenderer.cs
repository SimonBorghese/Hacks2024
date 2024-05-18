using UnityEngine;

public class GuardRenderer : MonoBehaviour
{
    public Transform Mesh;

    public PlayerController Controller;

    public Material FrontMaterial;

    public Material BackMaterial;

    public Material RightMaterial;

    public Material LeftMaterial;

    public Material DetectedMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Mesh.LookAt(Controller.transform.position);
        Vector3 NewRotation = Mesh.eulerAngles;
        NewRotation.x = 0.0f;
        NewRotation.z = 0.0f;
        Mesh.eulerAngles = NewRotation;

        Vector3 Difference = transform.position - Controller.transform.position;

        float Dot = Vector3.Dot(Difference, transform.forward) / Vector3.Magnitude(Difference);
        
        float RightDot = Vector3.Dot(Difference, transform.right) / Vector3.Magnitude(Difference);
        if (Dot < 0.0)
        {
            Mesh.GetComponent<MeshRenderer>().material = FrontMaterial;
        }
        else if (Dot > -0.25 && Dot < 0.75)
        {
            if (RightDot > 0.0)
            {
                Mesh.GetComponent<MeshRenderer>().material = LeftMaterial;
            }
            else
            {
                Mesh.GetComponent<MeshRenderer>().material = RightMaterial;
            }
        }
        else
        {
            Mesh.GetComponent<MeshRenderer>().material = BackMaterial;
        }

        if (Controller.SuperSense)
        {
            Mesh.GetComponent<MeshRenderer>().material = DetectedMaterial;
        }
    }
}
