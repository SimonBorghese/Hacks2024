using System;
using UnityEngine;

public class PrefabObject : MonoBehaviour
{
    public PickupObject Object;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.CurrentObjectTarget = Object;
            Player.TargetObject = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.CurrentObjectTarget = null;
            Player.TargetObject = null;
        }
    }
}
