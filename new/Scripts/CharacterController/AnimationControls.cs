using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

namespace Insolence
{
    public class AnimationControls : MonoBehaviour
    {
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool isGrounded = true;
        [SerializeField] private bool wasGrounded = true;
        [SerializeField] private bool isJumping = false;
        [SerializeField] private bool canJump = false;
        [SerializeField] private bool isCrouching = false;
        [SerializeField] private KinematicCharacterMotor characterMotor;
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterControl characterControl;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            characterMotor = gameObject.GetComponent<KinematicCharacterMotor>();
            characterControl = GetComponent<CharacterControl>();
        }

        // Update is called once per frame
        void Update()
        {
            GetCharacterMotorInfo();

            SetMovingAnimation();
            SetJumpAnimation();
            SetCrouchAnimation();
            HandleFalling();
            wasGrounded = isGrounded;
        }

        private void SetMovingAnimation()
        {
            if (isMoving)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }

        private void SetJumpAnimation()
        {
            if (canJump)
            {
                animator.SetTrigger("Jump");
                animator.SetBool("IsJumping", isJumping);
            }
            else animator.SetBool("IsJumping", isJumping);
        }

        private void SetCrouchAnimation()
        {
            animator.SetBool("IsCrouching", isCrouching);
        }

        private void HandleFalling()
        {
            animator.SetBool("IsGrounded", isGrounded);
        }
        private void GetCharacterMotorInfo()
        {
            isMoving = characterControl.GetMoveInputVector().sqrMagnitude > 0 ? true : false;
            isGrounded = characterMotor.GroundingStatus.IsStableOnGround;
            isJumping = !characterMotor.GroundingStatus.IsStableOnGround && characterControl.jumpConsumed;
            canJump = !characterMotor.GroundingStatus.IsStableOnGround && characterControl.jumpConsumed && wasGrounded;
            isCrouching = characterControl.isCrouching;
        }
    }
}