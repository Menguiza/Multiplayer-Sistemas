using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PenguinMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // The movement speed of the penguin
    [SerializeField] private SpriteRenderer rend, fishRend;
    [SerializeField] private Animator anim;
    
    private Rigidbody2D rb; // The rigidbody component of the penguin
    private bool movementEnable = true;
    private float horizontalInput, verticalInput;

    public Animator Anim { get => anim; }
    public SpriteRenderer Rend { get=>rend; }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the rigidbody component of the penguin
    }

    private void FixedUpdate()
    {
        if(movementEnable) Movement();
    }

    public void DisableMovment()
    {
        movementEnable = false;
        horizontalInput = 0;
        verticalInput = 0;
        SetAnim(new Vector2(0,0));
    }

    public void EnableMovement()
    {
        movementEnable = true;
    }

    private void Movement()
    {
        // Get the horizontal and vertical input from the player
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the movement vector based on the input and movement speed
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * speed * Time.deltaTime;

        // Move the penguin in the desired direction
        rb.MovePosition(rb.position + movement);

        FlipSprite(movement, rend);
        FlipSprite(movement, fishRend);
        SetAnim(movement);
    }

    private void FlipSprite(Vector2 movement, SpriteRenderer renderer)
    {
        // Flip the sprite horizontally if moving left, otherwise flip it back to original orientation
        if (movement.x < 0)
        {
            renderer.flipX = true;
        }
        else if(movement.x > 0)
        {
            renderer.flipX = false;
        }
    }

    private void SetAnim(Vector2 movement)
    {
        if(movement.x != 0 || movement.y != 0) anim.SetFloat("Movement", 1);
        else anim.SetFloat("Movement", 0);
    }
}
