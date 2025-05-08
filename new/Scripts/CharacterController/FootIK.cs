using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    /// <summary>
    /// Adjusts character model height and feet position and rotation so they touch the ground
    /// </summary>
    public class FootIK : MonoBehaviour
    {
        float ikWeight = 1f; //ik will completely override animation
        private Animator _animator;
        [SerializeField] private float footPlacementOffset = 0.06f;
        [SerializeField] private float animationOffset = 0.04f; //????

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GameObject characterBase;
        [SerializeField] private GameObject modelRoot;
        [SerializeField] private GameObject leftFootBone;
        [SerializeField] private GameObject rightFootBone;
        [SerializeField] private float currentAdjustMultiplier = 0f;
        [SerializeField] private float maxAdjustDistance = 0.75f;
        [SerializeField] private float adjustLerpSpeed = 3f;
        [SerializeField] private float ikRaycastDistance = 5f;
        [SerializeField] private float lerpedMult = 0f;
        [SerializeField] private float multReduction = 0.5f;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            // List of idle animation names to check against
            bool isIdleAnimation = _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Human@Idle01") ||
                                   _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Human@Idle01_Variant01") ||
                                   _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Human@Idle01-To-Variant01") ||
                                   _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Human@Idle01_Variant01-To-Idle01") ||
                                   _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Crouch_idle");


            // Do this only when one of the idle animations is playing
            if (isIdleAnimation)
            {
                currentAdjustMultiplier = _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Crouch_idle") ? 0f : lerpedMult;

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikWeight);

                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikWeight);

                Vector3 rayDir = Vector3.down;

                

                // Adjust IK for left and right foot
                AdjustFootIK(AvatarIKGoal.LeftFoot, rayDir);
                AdjustFootIK(AvatarIKGoal.RightFoot, rayDir);

                // Adjust modelRoot's height based on foot positions and tolerance
                AdjustModelHeight();
            }
            else modelRoot.transform.position = characterBase.transform.position - new Vector3 (0f, animationOffset, 0f);
        }

        /// <summary>
        /// Adjusts feet to ground layer
        /// </summary>
        /// <param name="ikGoal"></param>
        /// <param name="rayDir"></param>
        private void AdjustFootIK(AvatarIKGoal ikGoal, Vector3 rayDir)
        {
            Vector3 rayStartPos = _animator.GetIKPosition(ikGoal) + Vector3.up;
            bool isGround = Physics.Raycast(rayStartPos, rayDir, out RaycastHit hitInfo, ikRaycastDistance, groundLayer);

            if (isGround)
            {
                Vector3 hitPos = hitInfo.point + hitInfo.normal * footPlacementOffset;
                _animator.SetIKPosition(ikGoal, hitPos);

                Quaternion lookDir = Quaternion.LookRotation(transform.forward, hitInfo.normal);
                _animator.SetIKRotation(ikGoal, lookDir);


            }
        }
        /// <summary>
        /// Adjusts model height if feet are floating
        /// </summary>
        private void AdjustModelHeight()
        {
            // Calculate distances from each foot bone to its IK target
            float leftFootDistance = Vector3.Distance(leftFootBone.transform.position, _animator.GetIKPosition(AvatarIKGoal.LeftFoot));
            float rightFootDistance = Vector3.Distance(rightFootBone.transform.position, _animator.GetIKPosition(AvatarIKGoal.RightFoot));

            // Determine the desired adjustment based on which foot is outside tolerances
            float adjustmentYL = _animator.GetIKPosition(AvatarIKGoal.LeftFoot).y - leftFootBone.transform.position.y - footPlacementOffset;
            float adjustmentYR = _animator.GetIKPosition(AvatarIKGoal.RightFoot).y - rightFootBone.transform.position.y - footPlacementOffset;
            float adjustmentY = Mathf.Min(adjustmentYL, adjustmentYR);

            // Calculate Difference in needed adjustments
            float adjustmentDiff = Mathf.Abs(adjustmentYL - adjustmentYR);

            // Map the adjustment diff to between 1 and 3
            lerpedMult = Mathf.Lerp(1f, 2f, adjustmentDiff / maxAdjustDistance);

            // Check for ground between foot and knee to prevent knee penetration
            bool groundBetweenFootAndKnee = IsGroundBetweenFootAndKnee(AvatarIKGoal.LeftFoot) || IsGroundBetweenFootAndKnee(AvatarIKGoal.RightFoot);

            // Adjust multReduction dynamically
            if (groundBetweenFootAndKnee)
            {
                // Increase multReduction towards 0.1 (more reduction)
                multReduction = Mathf.MoveTowards(multReduction, 0.1f, Time.deltaTime * adjustLerpSpeed);
            }
            else
            {
                // Decrease multReduction towards 1 (less reduction)
                multReduction = Mathf.MoveTowards(multReduction, 2.5f, Time.deltaTime * adjustLerpSpeed);
            }


            // Ensure multReduction stays within 0.5 to 1 range
            multReduction = Mathf.Clamp(multReduction, 0.1f, 2.5f);

            // Apply multReduction to lerpedMult
            float adjustedLerpedMult = lerpedMult * multReduction;

            // Calculate the total adjustment
            float totalAdjustmentY = adjustmentY * adjustedLerpedMult;

            // Clamp the total adjustment to the maximum allowed value
            totalAdjustmentY = Mathf.Clamp(totalAdjustmentY, -maxAdjustDistance, maxAdjustDistance);

            // Smooth the adjustment to prevent vibration, interpolating toward the target height
            Vector3 targetPosition = modelRoot.transform.position;
            targetPosition.y = characterBase.transform.position.y + totalAdjustmentY;

            modelRoot.transform.position = Vector3.Lerp(modelRoot.transform.position, targetPosition, Time.deltaTime * adjustLerpSpeed);

        }
        /// <summary>
        /// Helper method to check if there is ground between the foot IK and knee
        /// </summary>
        /// <param name="ikGoal"></param>
        /// <returns></returns>
        private bool IsGroundBetweenFootAndKnee(AvatarIKGoal ikGoal)
        {
            // Get the foot and knee positions directly from the animator
            Vector3 footPosition = _animator.GetIKPosition(ikGoal);
            Vector3 kneePosition = ikGoal == AvatarIKGoal.LeftFoot
                ? _animator.GetIKHintPosition(AvatarIKHint.LeftKnee)
                : _animator.GetIKHintPosition(AvatarIKHint.RightKnee);

            // Raycast from foot to knee
            return Physics.Raycast(footPosition, (kneePosition - footPosition).normalized, out RaycastHit hitInfo, Vector3.Distance(footPosition, kneePosition), groundLayer);
        }

    }
}
