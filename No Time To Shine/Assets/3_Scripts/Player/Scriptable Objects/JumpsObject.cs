using UnityEngine;

public abstract class JumpsObject : ScriptableObject
{
    public abstract void Jump(MonoBehaviour monoBehaviour, bool isJumping);

    public abstract void GetJumpComponents(Rigidbody2D rb, GameObject feet);

}
