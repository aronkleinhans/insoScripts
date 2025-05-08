using UnityEngine;
using KinematicCharacterController;

namespace Insolence
{
    public class NoSnowPathOnJump : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private KinematicCharacterMotor _characterMotor;
        [SerializeField] private bool _isStaticObject;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStaticObject && _characterMotor)
            {
                if (!_characterMotor.GroundingStatus.IsStableOnGround && _particleSystem.isPlaying)
                {
                    _particleSystem.Pause();
                }
                else if (_characterMotor.GroundingStatus.IsStableOnGround && _particleSystem.isPaused)
                {
                    _particleSystem.Play();
                }
            }

        }
    }
}