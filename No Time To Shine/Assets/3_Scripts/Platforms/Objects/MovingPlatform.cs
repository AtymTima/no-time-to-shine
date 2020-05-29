using UnityEngine;

[CreateAssetMenu(menuName = "My Platforms/Moving")]
public class MovingPlatform : Platform
{
    [SerializeField]
    private float fallSpeed = 1f;
    private float targetDirection = 1f;
    private GameObject currentPlatform;

    private Vector2 currentPos;
    private Vector2 targetPos;

    //private void OnEnable()
    //{
    //    PlatformCollision.onWallCollided += ChangeDirection;
    //}

    //private void OnDisable()
    //{
    //    PlatformCollision.onWallCollided -= ChangeDirection;
    //}

    public override void SetPlatformSpeed(float moveSpeed, bool positiveDirection, GameObject gameObject)
    {
        //fallSpeed = moveSpeed;
        //targetDirection = positiveDirection ? 1 : -1;
        targetDirection = 1;
    }

    public override void MovePlatform(GameObject gameObject, bool directionIsReversed)
    {
        currentPos = gameObject.transform.localPosition;
        targetPos.x = currentPos.x + targetDirection * (directionIsReversed ? 1 : -1);
        targetPos.y = currentPos.y;
        gameObject.transform.localPosition = Vector2.MoveTowards(currentPos, targetPos, fallSpeed * Time.deltaTime);
    }

    private void ChangeDirection(Transform transform, int platformIndex)
    {
        targetDirection = -targetDirection;
    }
}
