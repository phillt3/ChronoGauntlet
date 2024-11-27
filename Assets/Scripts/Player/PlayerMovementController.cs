using System.Collections;
using UnityEngine;

//This class is responsible for tracking and executing all player movement mechanics
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float dashDistance = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public float jumpForce = 7f;

    private Rigidbody rb;
    private Animator anim;
    private Transform cam;

    Vector3 camForward;
    Vector3 moveInput;
    float forwardAmount;
    float turnAmount;

    //The Double Jump feature is turned on with a pickup
    private bool _hasDJump = false;
    public bool HasDJump{
        get { return _hasDJump; }
        set { _hasDJump = value; }
    }

    private bool _isDashing = false;
    public bool IsDashing 
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }

    //The Dash feature is turned on with a pickup
    private bool _hasDash = false;
    public bool HasDash{
        get { return _hasDash; }
        set { _hasDash = value; }
    }

    private bool canDash = true; //this flag handles whether or not a player can dash with a cooldown
    private float dashTime;
    private Vector3 dashDirection;

    public LayerMask groundLayer;
    private bool isGrounded;
    public float groundCheckDistance = 0.2f;
    private Vector3 groundCheckOffset = new Vector3(0, 0.1f, 0);
    private int jumpCount = 0;
    public int JumpCount 
    {
        get => jumpCount;
    }

    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip walkSound;
    public AudioClip waterWalkSound;

    private AudioSource audioSource;
    private bool isWalking = false;
    private bool isInWater = false;

    private float originalMoveSpeed;
    private float originalRotationSpeed;
    private float originalDashDistance;
    private float originalJumpForce;

    private Transform currentPlatform; // Store the platform the player is standing on
    private Vector3 previousPlatformPosition; // Track the previous position of the platform to calculate its movement
    private Vector3 platformVelocity; // Store the calculated velocity of the platform

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.Log("rb is null.");
        }

        anim = GetComponent<Animator>();
        if (anim == null) {
            Debug.Log("anim is null.");
        }

        cam = Camera.main.transform;
        if (cam == null) {
            Debug.Log("cam is null.");
        }

        //Need to always have a track on original values so that they can be dampered and reset in water for example
        originalMoveSpeed = moveSpeed;
        originalRotationSpeed = rotationSpeed;
        originalDashDistance = dashDistance;
        originalJumpForce = jumpForce;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
        }
    }

    void FixedUpdate()
    {
        if (IsDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement();
        }
        ApplyPlatformMovement(); 
        CheckGroundStatus();
    }

    void Update()
    {
        HandleDashInput();
        HandleJumpInput();
    }

    //Calculate where the player should move based on input
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveInput = CalculateMoveInput(horizontal, vertical);

        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        Vector3 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            if (isGrounded && !isWalking)
            {
                StartCoroutine(PlayWalkSound());
            }
        } else {
            isWalking = false;
        }

        UpdateAnimator();
    }

    private IEnumerator PlayWalkSound()
    {
        isWalking = true;
        while (isWalking)
        {
            if (walkSound != null && isGrounded && !audioSource.isPlaying) // Ensure sound plays only when grounded
            {
                audioSource.PlayOneShot(walkSound);
                if (isInWater) {
                    Debug.Log("Playing Water Sound");
                    AudioSource.PlayClipAtPoint(waterWalkSound, transform.position);
                }
            }
            yield return new WaitForSeconds(0.5f); // Adjust interval to match walking animation
        }
    }

    private Vector3 CalculateMoveInput(float horizontal, float vertical)
    {
        if (cam != null)
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            return vertical * camForward + horizontal * cam.right;
        }
        else
        {
            return vertical * Vector3.forward + horizontal * Vector3.right;
        }
    }

    private void ApplyPlatformMovement()
    {
        if (currentPlatform != null)
        {
            // Calculate platform velocity based on position change from last frame
            platformVelocity = (currentPlatform.position - previousPlatformPosition) / Time.fixedDeltaTime;
            previousPlatformPosition = currentPlatform.position;

            // Apply platform's movement to player
            rb.MovePosition(rb.position + platformVelocity * Time.fixedDeltaTime);
        }
    }

    //Determine if the player is grounded after a jump
    private void CheckGroundStatus()
{
    RaycastHit hit;
    isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, out hit, groundCheckDistance, groundLayer);

    if (isGrounded)
    {
        if (!anim.GetBool("IsGrounded"))
        {
            if (landSound != null)
                AudioSource.PlayClipAtPoint(landSound, transform.position);
            anim.SetBool("IsGrounded", true);
            jumpCount = 0;
        }

        // Check if standing on a platform
        if (hit.transform.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.transform)
            {
                currentPlatform = hit.transform;
                previousPlatformPosition = currentPlatform.position; // Set initial platform position
            }
        }
        else
        {
            currentPlatform = null; // Reset if not on a platform
        }
    }
    else
    {
        anim.SetBool("IsGrounded", false);
        rb.AddForce(Physics.gravity * rb.mass);

        // Detach from platform when jumping
        currentPlatform = null;
    }
}

    //Inform animation of the movement and relative direction of player
    private void UpdateAnimator()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;

        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    private void HandleDashInput()
    {
        if (HasDash) {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !IsDashing && moveInput.magnitude > 0.1f)
            {
                StartDash();
            }
        }
    }

    //Determine if player is executing single jump or double jump
    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount > 1) {
                jumpCount++;
                return;
            } else if (jumpCount > 0 && HasDJump) {
                Jump();
            } else if (jumpCount < 1) {
                Jump();
            }
        }
    }

    //Perform jump calculations and animation
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;

        anim.SetTrigger("Jump");
        if (jumpSound != null)
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    //initiate dash and start animation
    private void StartDash()
    {
        IsDashing = true;
        canDash = false;

        dashDirection = moveInput.normalized;
        dashTime = 0f;

        rb.velocity = Vector3.zero;

        anim.SetTrigger("Dash");
        if (dashSound != null)
            AudioSource.PlayClipAtPoint(dashSound, transform.position);
    }

    //Handle mid dash calculations
    private void HandleDash()
    {
        dashTime += Time.fixedDeltaTime;
        if (dashTime < dashDuration)
        {
            Vector3 dashMovement = dashDirection * (dashDistance / dashDuration) * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + dashMovement);
        }
        else
        {
            EndDash();
        }
    }

    //End and reset dash
    private void EndDash()
    {
        IsDashing = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    //Dampen player movement while in water
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = true;
            moveSpeed = 2f;
            rotationSpeed = 5f;
            dashDistance = 1.5f;
            jumpForce = 7f;
        }
    }

    //Reset player movement after exiting water
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = false;
            moveSpeed = originalMoveSpeed;
            rotationSpeed = originalRotationSpeed;
            dashDistance = originalDashDistance;
            jumpForce = originalJumpForce;
        }
    }
}




