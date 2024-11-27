using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 720f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Animator animator;

    public bool isAttack;

    public GameObject attackTrigger;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (isAttack)
        {
            AttackAnimation();
        }
        else
        {
            MovePlayer();
            AnimatePlayer();
            StopAttackAnimation();
        }
        
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * vertical + right * horizontal;

        // Rotate player if there is movement
        if (move.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), turnSpeed * Time.deltaTime);
        }

        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    void AnimatePlayer()
    {
        // Get the player's movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // If there is any movement, set the isRunning bool to true
        bool isMoving = horizontal != 0 || vertical != 0;
        animator.SetBool("isRunning", isMoving);
    }
    private void AttackAnimation()
    {
        animator.SetBool("Attack", true);
    }
    private void StopAttackAnimation()
    {
        animator.SetBool("Attack", false);
    }
    public void Attack()
    {
        isAttack = true;
    }

    public void StopAttack()
    {
        isAttack = false;
    }

    public void OnAttackTrigger()
    {
        attackTrigger.SetActive(true);
    }
    public void OffAttackTrigger()
    {
        attackTrigger.SetActive(false);
    }

}
