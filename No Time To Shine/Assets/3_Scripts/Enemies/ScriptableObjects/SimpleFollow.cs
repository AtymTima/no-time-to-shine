using UnityEngine;

[CreateAssetMenu(menuName = "My Enemy/Movement/Simple Follow")]
public class SimpleFollow : MovementAI
{
    [Header("Movement")]
    [SerializeField] float followSpeed = 0.1f;
    [SerializeField] float stoppingDistance = 0.5f;
    [SerializeField] float chasingSpeed =0.5f;
    [SerializeField] float rotationMultiplier = 10;

    [Header("Boundaries")]
    [SerializeField] float bottomBoundary = -4.8f;
    [SerializeField] float topBoundary = 8;
    [SerializeField] float timeBeforeChasing = 2.5f;

    Vector2 currentPos;
    Vector3 currentRotation;
    Transform aiTransform;
    bool hasAppeared;
    float currentSpeed;
    float currentAngle;

    float timeRemains;

    bool chasingStarted;

    public override void SetInitialParams(GameObject gameObject)
    {
        currentRotation = gameObject.transform.eulerAngles;
        timeRemains = timeBeforeChasing;
        chasingStarted = false;
    }

    public override void MoveAI(GameObject gameObject, Transform target, bool isLightOn)
    {
        aiTransform = gameObject.transform;
        if (Vector2.Distance(aiTransform.localPosition, target.localPosition) > stoppingDistance)
        {
            if (chasingStarted)
            {
                currentSpeed = isLightOn ? chasingSpeed : followSpeed;
            }
            else
            {
                currentSpeed = 0.1f;
                IsStartChasing();
            }

            currentPos = aiTransform.localPosition;
            currentPos.y = Mathf.Clamp(currentPos.y, bottomBoundary, topBoundary);
            aiTransform.localPosition = Vector2.MoveTowards(currentPos, target.localPosition, currentSpeed * Time.deltaTime);

            currentAngle = aiTransform.localPosition.x > target.localPosition.x ? rotationMultiplier : -rotationMultiplier;
            currentRotation.z = currentSpeed * currentAngle * Time.deltaTime;
            aiTransform.Rotate(currentRotation);
        }
    }

    private void IsStartChasing()
    {
        if (timeRemains > 0)
        {
            timeRemains -= Time.deltaTime;
        }
        else
        {
            chasingStarted = true;
        }
    }

    public override void StartExpand(bool isExpand)
    {
        throw new System.NotImplementedException();
    }
}
