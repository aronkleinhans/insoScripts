using UnityEngine;

public class WaterController : MonoBehaviour
{
    public  static WaterController current;

    public bool isMoving;

    // Wave height and speed
    public float speed = 1.0f;
    public float scale = 0.1f;
    // The width between the waves
    public float waveDistance = 1f;
    // Noise parameters
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;

    void Start()
    {
        current = this;    
    }

    // Get the Y coordinate from whatever wavetype we're using
    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        return 0f;
    }

    // Find the distance from a vertice to water
    /// <summary>
    /// Find the distance from a vertice to water
    /// </summary>
    /// <param name="position"></param>
    /// <param name="timeSinceStart"></param>
    /// <returns> float (positive if above water, negative if not) </returns>
    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float waterHeight = GetWaveYPos(position, timeSinceStart);
        float distanceToWater = position.y - waterHeight;
        return distanceToWater;
    }

}
