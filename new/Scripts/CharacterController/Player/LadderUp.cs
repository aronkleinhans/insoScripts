using UnityEngine;
using KinematicCharacterController;
using Insolence;

/// <summary>
/// Debug script to move player to target location(in this case top of a ladder)
/// </summary>
public class LadderUp : MonoBehaviour
{
    [SerializeField] private GameObject _targetLocation;
    private BoxCollider _boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector3(1f, 5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CharacterControl cc = other.GetComponent<CharacterControl>();
                if (cc)
                {
                    cc.Motor.SetPositionAndRotation(_targetLocation.transform.position, _targetLocation.transform.rotation);

                }
            }
        }
    }
}
