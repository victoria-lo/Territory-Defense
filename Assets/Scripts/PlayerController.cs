using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public AudioClip jump;
    AudioSource sfx;
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    private Animator anim;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool grounded;
    private bool turn, attack, disableLeft, disableRight, isFacingRight;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        if (GetComponentInChildren<Animator>() != null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim.GetComponent<Transform>().name == "hero(Clone)" || anim.GetComponent<Transform>().name == "scholar(Clone)")
            {
                isFacingRight = true;
            }
        }
    }

    private void Update()
    {
        if (!attack)
        {
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            float moveInput = 0;
            if (turn)
                moveInput = Input.GetAxisRaw("Horizontal");
            else anim.SetBool("walk", false);

            if (grounded)
            {
                velocity.y = 0;

                if (Input.GetButtonDown("Jump") && turn)
                {
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    sfx.PlayOneShot(jump);
                    anim.SetBool("jump", true);
                }
            }
            else if (velocity.y < 0) //free falling
            {
                anim.SetBool("jump", true);
            }

            float acceleration = grounded ? walkAcceleration : airAcceleration;
            float deceleration = grounded ? groundDeceleration : 0;

            if (moveInput != 0)
            {
                Transform warrior = anim.GetComponent<Transform>();
                if (moveInput < 0 && !isFacingRight)
                {
                    warrior.localScale = new Vector3(warrior.localScale.x * -1, warrior.localScale.y, warrior.localScale.z);
                    isFacingRight = true;
                }
                if (moveInput > 0 & isFacingRight)
                {
                    warrior.localScale = new Vector3(warrior.localScale.x * -1, warrior.localScale.y, warrior.localScale.z);
                    isFacingRight = false;
                }
                if (moveInput < 0 & disableRight) //moving right
                    moveInput = 0;
                else if (moveInput > 0 & disableLeft) //moving left
                    moveInput = 0;
                velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
                anim.SetBool("walk", true);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
                anim.SetBool("walk", false);
            }

            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);

            grounded = false;

            // Retrieve all colliders we have intersected after velocity has been applied.
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

            foreach (Collider2D hit in hits)
            {
                // Ignore our own collider.
                if (hit == boxCollider)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                if (colliderDistance.isOverlapped)
                {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                    // If we intersect an object beneath us, set grounded to true. 
                    if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                    {
                        grounded = true;
                        anim.SetBool("jump", false);
                    }
                }
            }
        }
    }

    public void setTurn(bool set)
    {
        turn = set;
    }

    public void setAttack(bool set)
    {
        attack = set;
    }

    public void setMaxX(bool set)
    {
        disableRight = set;
    }

    public void setMinX(bool set)
    {
        disableLeft = set;
    }
}
