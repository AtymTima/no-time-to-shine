using UnityEngine;

[CreateAssetMenu(menuName = "My Enemy/Movement/Torch Keeper")]
public class TorchKeeper : MovementAI
{
    [Header("Movement")]
    [SerializeField] float roundingSpeed = 5f;
    [SerializeField] float stoppingDistanceMin = 5f;
    [SerializeField] float stoppingDistanceMax = 15f;

    [SerializeField] float relocateSpeed = 10f;
    [SerializeField] float rotationMultiplier = 10;

    [Header("Boundaries")]
    [SerializeField] float bottomBoundary = -4.8f;
    [SerializeField] float topBoundary = 8;
    [SerializeField] float timeBeforeRelocation = 2f;
    [SerializeField] float scaleMultiplier = 5f;

    Vector2 currentPos;
    Vector3 currentRotation;
    Vector2 currentScale;

    Transform aiTransform;
    float currentSpeed;
    float currentAngle;

    float timeRemains;

    bool isExpand;
    bool isShrink;
    bool minimumIsReached;

    public override void SetInitialParams(GameObject gameObject)
    {
        currentRotation = gameObject.transform.eulerAngles;
        currentScale = gameObject.transform.localScale;
        timeRemains = timeBeforeRelocation;
        isShrink = false;
        isExpand = false;
        minimumIsReached = false;
    }

    public override void MoveAI(GameObject gameObject, Transform target, bool isLightOn)
    {
        aiTransform = gameObject.transform;

        currentAngle = aiTransform.localPosition.x > target.localPosition.x ? rotationMultiplier : -rotationMultiplier;
        currentRotation.z = currentSpeed * currentAngle * Time.deltaTime;
        aiTransform.Rotate(currentRotation);

        if (isExpand)
        {
            ExpandSize();
            return;
        }

        if (isShrink)
        {
            ShrinkSize();
            return;
        }

        if (!minimumIsReached)
        {
            if (Vector2.Distance(aiTransform.localPosition, target.localPosition) > stoppingDistanceMin)
            {
                currentSpeed = relocateSpeed;
                currentPos = aiTransform.localPosition;
                currentPos.y = Mathf.Clamp(currentPos.y, bottomBoundary, topBoundary);
                aiTransform.localPosition = Vector2.MoveTowards(currentPos, target.localPosition, currentSpeed * Time.deltaTime);
            }
            else
            {
                minimumIsReached = true;
            }
        }
        else
        {
            if (Vector2.Distance(aiTransform.localPosition, target.localPosition) < stoppingDistanceMax)
            {
                currentSpeed = roundingSpeed;
                currentPos = target.position;
                aiTransform.RotateAround(currentPos, Vector3.forward, currentSpeed * Time.deltaTime);
                minimumIsReached = true;
            }
            else
            {
                minimumIsReached = false;
            }
        }

    }

    public override void StartExpand(bool isExpand)
    {
        this.isExpand = true;
    }

    private void ExpandSize()
    {
        if (currentScale.x >= 2)
        {
            isExpand = false;
            isShrink = true;
            timeRemains = timeBeforeRelocation;
            return;
        }
        currentScale.x += Time.deltaTime * scaleMultiplier;
        currentScale.y += Time.deltaTime * scaleMultiplier;
        aiTransform.localScale = currentScale;
    }

    private void ShrinkSize()
    {
        if (!IsTimeElapsed()) { return; }
        if (currentScale.x <= 1 || currentScale.y <= 1)
        {
            isShrink = false;
            return;
        }
        currentScale.x -= Time.deltaTime * scaleMultiplier;
        currentScale.y -= Time.deltaTime * scaleMultiplier;
        aiTransform.localScale = currentScale;
    }

    private bool IsTimeElapsed()
    {
        if (timeRemains >= Mathf.Epsilon)
        {
            timeRemains -= Time.deltaTime;
            return false;
        }
        return true;
    }
}
