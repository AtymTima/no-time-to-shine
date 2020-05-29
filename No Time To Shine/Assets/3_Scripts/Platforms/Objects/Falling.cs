using UnityEngine;

[CreateAssetMenu(menuName ="My Platforms/Falling")]
public class Falling : Platform
{
    [SerializeField]
    private float fallSpeed = 5f;
    private int targetDirection;
    private GameObject currentPlatform;
    private Vector2 currentPos;
    private Vector2 targetPos;

    public override void SetPlatformSpeed(float moveSpeed, bool positiveDirection, GameObject gameObject)
    {
        //fallSpeed = moveSpeed;
        targetDirection = positiveDirection ? 1 : -1;
        currentPlatform = gameObject;
    }

    public override void MovePlatform(GameObject gameObject, bool directionIsReversed)
    {
        currentPos = gameObject.transform.localPosition;
        targetPos.x = currentPos.x;
        targetPos.y = currentPos.y - targetDirection;
        gameObject.transform.localPosition = Vector2.MoveTowards(currentPos, targetPos, fallSpeed * Time.deltaTime);
    }
}
