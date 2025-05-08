using UnityEngine;

public class SnowEmitterFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 playerPosition;
    [SerializeField] Vector3 emitterPosition;
    [SerializeField] float emitterYOffset = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get position data
        emitterPosition = transform.position;
        playerPosition = player.transform.position;

        // Create a target position that includes the offset
        Vector3 targetPosition = new Vector3(
            playerPosition.x,
            playerPosition.y + emitterYOffset,
            playerPosition.z
        );

        if (emitterPosition != targetPosition)
        {
            emitterPosition = targetPosition;
            transform.position = emitterPosition;
        }
    }
}
