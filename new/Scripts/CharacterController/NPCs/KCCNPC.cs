using UnityEngine;
using UnityEngine.AI;

namespace Insolence
{
    public class KCCNPC : MonoBehaviour
    {
        public CharacterControl character;
        public GameObject targetGO;

        private Vector3 target;
        private NavMeshAgent agent;
        //private NPCAIController npc;

        void OnEnable()
        {
            agent = GetComponentInParent<NavMeshAgent>();
            character = GetComponentInParent<CharacterControl>();
            //npc = GetComponent<NPCAIController>();
        }

        void Update()
        {

            if (targetGO != null)
            {
                target = targetGO.transform.position;
            }
            MoveTo(target);
            //TODO ignore Y??
            ApplyInputs(agent.velocity);

            //update the max speed of the character based on distance to target
            if (Vector3.Distance(transform.position, target) > 20f)
            {
                character.MaxStableMoveSpeed = 10f;
            }
            else
            {
                character.MaxStableMoveSpeed = 2f;
            }

        }

        private void ApplyInputs(Vector3 input)
        {
            AICharacterInputs inputs = new AICharacterInputs();

            //set the KKC inputs from navmesh agent velocity

            inputs.MoveVector = new Vector3(input.x, 0f, input.z);

            inputs.LookVector = new Vector3(input.x, 0f, input.z);

            character.SetInputs(ref inputs);

        }
        public void MoveTo(Vector3 target)
        {
            if (agent.destination != target)
            {
                agent.SetDestination(target);
            }

            this.target = target;

        }

        public void Stop()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        public Vector3 GetDestination()
        {
            return agent.destination;
        }

        public bool AtDestination()
        {
            if (agent.destination != null && Vector3.Distance(transform.position, agent.destination) < 1.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
