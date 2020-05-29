using UnityEngine;

[CreateAssetMenu(menuName ="My Player/Movement/Running")]
public class Running : MovementObject
{
    [SerializeField] float speedMovement = 1f;

    private Rigidbody2D rb;
    private Vector2 movementVector;

    public override void Move(MonoBehaviour monoBehaviour, float direction)
    {
        if (rb == null)
        {
            rb = monoBehaviour.GetComponent<Rigidbody2D>();
        }

        MovePlayer(direction);
    }

    private void MovePlayer(float direction)
    {
        movementVector.x = direction * speedMovement * Time.fixedDeltaTime;
        movementVector.y = rb.velocity.y;
        rb.velocity = movementVector;
    }
}
