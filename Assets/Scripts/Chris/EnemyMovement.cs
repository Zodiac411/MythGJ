using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Fields and Properties
    public enum MovementType { Vertical, Horizontal, Zigzag }
    public MovementType movementType;

    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private float speed;

    [SerializeField]
    private float zigzagFrequency = 2f;
    [SerializeField]
    private float zigzagMagnitude = 0.5f;
    private Vector3 direction;
    private IMovementStrategy movementStrategy;
    #endregion

    #region Unity Methods
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        movementStrategy = CreateMovementStrategy();
        movementStrategy.Initialize(this);
    }

    void Update()
    {
        if (movementStrategy != null)
        {
            movementStrategy.Move(this, Time.deltaTime);
        }
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private IMovementStrategy CreateMovementStrategy()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                return new HorizontalMovementStrategy();
            case MovementType.Zigzag:
                return new ZigzagMovementStrategy();
            default:
                return new VerticalMovementStrategy();
        }
    }

    private interface IMovementStrategy
    {
        void Initialize(EnemyMovement enemy);
        void Move(EnemyMovement enemy, float deltaTime);
    }

    private sealed class VerticalMovementStrategy : IMovementStrategy
    {
        public void Initialize(EnemyMovement enemy)
        {
            enemy.direction = Vector3.down;
        }

        public void Move(EnemyMovement enemy, float deltaTime)
        {
            enemy.transform.Translate(Vector3.down * enemy.speed * deltaTime, Space.World);
        }
    }

    private sealed class HorizontalMovementStrategy : IMovementStrategy
    {
        public void Initialize(EnemyMovement enemy)
        {
            enemy.direction = enemy.transform.position.x < 0 ? Vector3.right : Vector3.left;
        }

        public void Move(EnemyMovement enemy, float deltaTime)
        {
            enemy.transform.Translate(enemy.direction * enemy.speed * deltaTime, Space.World);
        }
    }

    private sealed class ZigzagMovementStrategy : IMovementStrategy
    {
        public void Initialize(EnemyMovement enemy)
        {
            enemy.direction = enemy.transform.position.y > 0
                ? Vector3.down
                : (enemy.transform.position.x > 0 ? Vector3.left : Vector3.right);
        }

        public void Move(EnemyMovement enemy, float deltaTime)
        {
            enemy.transform.position += enemy.direction * enemy.speed * deltaTime;

            if (enemy.direction == Vector3.down)
            {
                enemy.transform.position += Vector3.right * Mathf.Sin(Time.time * enemy.zigzagFrequency) * enemy.zigzagMagnitude;
            }
            else
            {
                enemy.transform.position += Vector3.down * Mathf.Sin(Time.time * enemy.zigzagFrequency) * enemy.zigzagMagnitude;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag ==  "Player") {
            collision.gameObject.GetComponent<PlayerController>().DealDamage(1);
            Destroy(gameObject);
        }
    }
    #region Movement Methods
    bool IsOffScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        return screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1;
    }

    
    #endregion

    #region Inspector Methods
    // This region can be used for methods that are specifically meant to be called or shown in the Unity Inspector if needed in the future.
    #endregion
}
