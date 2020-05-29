using UnityEngine;

[CreateAssetMenu(menuName ="My Platforms/Rising")]
public class RisingPlatform : Platform
{
    [SerializeField]
    private float riseSpeed = 5f;
    private int targetDirection;
    private GameObject currentPlatform;
    private Vector2 currentPos;
    private Vector2 targetPos;

    public override void SetPlatformSpeed(float moveSpeed, bool positiveDirection, GameObject gameObject)
    {
        targetDirection = positiveDirection ? 1 : -1;
        currentPlatform = gameObject;
    }

    public override void MovePlatform(GameObject gameObject, bool directionIsReversed)
    {
        currentPos = gameObject.transform.localPosition;
        targetPos.x = currentPos.x;
        targetPos.y = currentPos.y + targetDirection;
        gameObject.transform.localPosition = Vector2.MoveTowards(currentPos, targetPos, riseSpeed * Time.deltaTime);
    }
}
