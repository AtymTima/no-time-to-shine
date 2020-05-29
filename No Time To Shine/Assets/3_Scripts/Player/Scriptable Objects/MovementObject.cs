using UnityEngine;

public abstract class MovementObject : ScriptableObject
{
    public abstract void Move(MonoBehaviour monoBehaviour, float direction);
}
