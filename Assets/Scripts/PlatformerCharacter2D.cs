using System;
using UnityEngine;


public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public float m_MaxPickupDistance = 2;
    public float m_HoldDistance = 1;

    private GameObject m_OverBuilding = null;

    public GameObject CarryingObject { get; private set; }
    public bool IsCarryingObject { get { return CarryingObject != null; } }

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        if (CarryingObject != null) {
            var offset = transform.right * m_HoldDistance;
            if (!m_FacingRight) {
                offset = -offset;
            }
            CarryingObject.transform.position = this.transform.position + offset;
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void AttemptPickUp() {
        var pickupableObjects = GameObject.FindGameObjectsWithTag("Pickupable");

        var characterPos = this.transform.position;

        GameObject best = null;
        float bestDistance = -1;
        foreach (var o in pickupableObjects) {
            var distance = (o.transform.position - characterPos).magnitude;
            if (distance < m_MaxPickupDistance && (best == null || distance < bestDistance)) {
                best = o;
                bestDistance = distance;
            }
        }

        if (best != null) {
            CarryingObject = best;
            CarryingObject.GetComponent<Rigidbody2D>().isKinematic = true;
            CarryingObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            ExamineUI.Instance.Object = best;
			ExamineUI.Instance.Visible = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnter " + other.gameObject.name);
        if (other.gameObject.tag == "Architecture") {
            m_OverBuilding = other.gameObject;
        } else if (!IsCarryingObject && other.gameObject.tag == "PickupableTrigger") {
			ExamineUI.Instance.Object = other.transform.parent.gameObject;
			ExamineUI.Instance.Visible = true;
		}
	}

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Architecture") {
            m_OverBuilding = null;
        } else if (!IsCarryingObject && other.gameObject.tag == "PickupableTrigger") {
            ExamineUI.Instance.Object = null;
            ExamineUI.Instance.Visible = false;
        }

    }

    public void Drop() {
        if (IsCarryingObject) {
            var o = CarryingObject;
            if (m_OverBuilding != null) {
                if (m_OverBuilding.GetComponent<DistributionHQ>() != null) {
                    DistributeUI.Instance.Object = o;
                    DistributeUI.Instance.Visible = true;
                } else if (m_OverBuilding.GetComponent<MarketResearchHQ>() != null) {
                    MarketResearchUI.Instance.Object = o;
                    MarketResearchUI.Instance.Visible = true;
                }
            } else {
                CarryingObject = null;
                o.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }

}

