using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PenguinMovement : MonoBehaviourPun
{
    [SerializeField] private float speed = 5f; // The movement speed of the penguin
    [SerializeField] private SpriteRenderer rend, fishRend;
    [SerializeField] private Animator anim;
    [SerializeField] private PhotonView playerObjView;
    
    private Rigidbody2D rb; // The rigidbody component of the penguin
    private bool movementEnable = true;
    private float horizontalInput, verticalInput;
    private PhotonView pv;

    public Animator Anim { get => anim; }
    public SpriteRenderer Rend { get=>rend; }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the rigidbody component of the penguin
        pv = GetComponentInParent<PhotonView>();
        
        anim.SetBool("Flip", rend.flipX);
    }

    private void FixedUpdate()
    {
        if (movementEnable)
        {
            if(pv.IsMine) Movement();
            rend.flipX = anim.GetBool("Flip");
            fishRend.flipX = anim.GetBool("Flip");
        }
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

        FlipSprite(movement);
        SetAnim(movement);
    }
    
    private void FlipSprite(Vector2 movement)
    {
        // Flip the sprite horizontally if moving left, otherwise flip it back to original orientation
        if (movement.x < 0)
        {
            anim.SetBool("Flip", true);
        }
        else if(movement.x > 0)
        {
            anim.SetBool("Flip", false);
        }
    }

    private void SetAnim(Vector2 movement)
    {
        if(movement.x != 0 || movement.y != 0) anim.SetFloat("Movement", 1);
        else anim.SetFloat("Movement", 0);
    }
}
