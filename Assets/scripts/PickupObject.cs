using UnityEngine;

[CreateAssetMenu(fileName = "PickupObject", menuName = "Scriptable Objects/PickupObject")]
public class PickupObject : ScriptableObject
{
    public enum ObjectType
    {
        Throwable,
        Evidence
    };

    public ObjectType Type;
    public string Notice;
}
