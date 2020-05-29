using UnityEngine;

public abstract class Platform : ScriptableObject
{
    public abstract void SetPlatformSpeed(float moveSpeed, bool positiveDirection, GameObject gameObject);
    public abstract void MovePlatform(GameObject gameObject, bool directionIsReversed);
}
