using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Sprite[] sprites; // Assign this array in the inspector
    private SpriteRenderer spriteRenderer;
    private PassengerSpawner ownerSpawner;
    private float fallOffSpeed = 1f;
    private float shrinkRate = 0.5f;
    private float fadeRate = 1f;
    private bool isFallingOff = false;
    private float rotationSpeed = 180f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AssignRandomSprite();
    }

    private void AssignRandomSprite()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null && sprites.Length > 0)
        {
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }

    public void PrepareForSpawn(PassengerSpawner spawner, Vector3 position, bool flipX)
    {
        ownerSpawner = spawner;
        gameObject.SetActive(true);
        transform.position = position;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        spriteRenderer.flipX = flipX;
        spriteRenderer.color = Color.white;
        isFallingOff = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        AssignRandomSprite();
    }

    private void Update()
    {
        if (isFallingOff)
        {
            // Determine the fall direction based on the sprite's initial orientation
            float moveDirection = spriteRenderer.flipX ? -1 : 1; // Left for flipX true, right for false

            // Apply horizontal movement
            transform.Translate(Vector2.right * moveDirection * fallOffSpeed * Time.deltaTime);

            // Apply rotation; Positive rotationSpeed will rotate clockwise, negative will rotate counterclockwise
            // Ensure rotation direction is opposite to moveDirection to achieve the "mirror" effect
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime * moveDirection);

            // Shrink and fade to black
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkRate * Time.deltaTime);
            Color color = spriteRenderer.color;
            color.r = Mathf.Lerp(color.r, 0f, fadeRate * Time.deltaTime);
            color.g = Mathf.Lerp(color.g, 0f, fadeRate * Time.deltaTime);
            color.b = Mathf.Lerp(color.b, 0f, fadeRate * Time.deltaTime);
            spriteRenderer.color = color;

            // Destroy when barely visible
            if (transform.localScale.x < 0.05f)
            {
                if (ownerSpawner != null)
                {
                    ownerSpawner.RecyclePassenger(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void FallOff()
    {
        isFallingOff = true;
        /*fallOffSpeed *= spriteRenderer.flipX ? -1 : 1;
        rotationSpeed *= spriteRenderer.flipX ? -1 : 1;*/
        // Disable any colliders and gravity if you have them
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Stop physics interactions
        }
    }

}
