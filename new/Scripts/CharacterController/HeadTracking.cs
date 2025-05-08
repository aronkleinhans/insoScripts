using UnityEngine;

namespace Insolence
{
    public class HeadTracking : MonoBehaviour
    {
        [SerializeField] GameObject headTrackTarget;

        // Private variable to store the target object's transform
        [SerializeField] private Transform _target;
        [SerializeField] GameObject camTarget;
        [SerializeField] GameObject cameraFollowPoint;
        [SerializeField] private float headTrackingSpeed = 5;
        [SerializeField] bool targetGizmoOn = true;
        [SerializeField] float yOffset;
        [SerializeField] float xOffset;
        [SerializeField] float zOffset;

        MeshRenderer headtrackTargetMeshRenderer;

        // Public property to set and get the target object's transform
        public Transform Target
        {
            get { return _target; }
            set
            {
                // Set the target transform
                _target = value;
            }
        }

        private void Start()
        {
            //if camTarget == null create a new gameobject

            if (camTarget == null)
            {
                camTarget = new GameObject("camTarget");

                //set the newTarget position to where the camera is looking

                camTarget.transform.SetParent(gameObject.transform);
                camTarget.transform.position = headTrackTarget.transform.position;

            }

            if (headTrackTarget != null)
            {
                headtrackTargetMeshRenderer = headTrackTarget.GetComponent<MeshRenderer>();
            }
        }
        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                if (Camera.main.transform.position != cameraFollowPoint.transform.position)
                {
                    // Get the position of the camera relative to the cameraFollowPoint
                    Vector3 cameraOffset = Camera.main.transform.position - cameraFollowPoint.transform.position;

                    // Invert the X and Z components of the camera offset
                    Vector3 oppositeOffset = new Vector3(-cameraOffset.x + yOffset, -cameraOffset.y + xOffset, -cameraOffset.z + zOffset);

                    // Position the camTarget opposite to the camera
                    camTarget.transform.position = cameraFollowPoint.transform.position + oppositeOffset;

                }
            }
            if (Target != null)
            {
                // Move the headTrackTarget towards the target position using Lerp
                headTrackTarget.transform.position = Vector3.Lerp(headTrackTarget.transform.position, Target.transform.position, Time.deltaTime * headTrackingSpeed);
            }
            else
            {

                // If there is no target, move the headTrackTarget towards the camera target position using Lerp
                headTrackTarget.transform.position = Vector3.Lerp(headTrackTarget.transform.position, camTarget.transform.position, Time.deltaTime * headTrackingSpeed);
            }

            if (headTrackTarget && targetGizmoOn)
            {
                headtrackTargetMeshRenderer.enabled = true;
            }
            else
            {
                headtrackTargetMeshRenderer.enabled = false;
            }
        }
        
    }

}

