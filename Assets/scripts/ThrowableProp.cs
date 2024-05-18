using UnityEngine;

public class ThrowableProp : MonoBehaviour
{
    public PlayerController Player;

    public MeshRenderer Mesh;

    public Material BaseMaterial;

    public Material CloseMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        Mesh = GetComponent<MeshRenderer>();
        BaseMaterial = Mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.SuperSense)
        {
            Mesh.material = CloseMaterial;
        }
        else
        {
            Mesh.material = BaseMaterial;
        }
        
    }
}
