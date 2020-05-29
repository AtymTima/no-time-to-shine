using UnityEngine;

public abstract class MovementAI : ScriptableObject
{
    public abstract void MoveAI(GameObject gameObject, Transform target, bool isLightOn);
    public abstract void SetInitialParams(GameObject gameObject);
    public abstract void StartExpand(bool isExpand);
}
