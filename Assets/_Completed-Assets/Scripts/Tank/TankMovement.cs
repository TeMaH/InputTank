using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public float m_Speed = 12f;
        public float m_TurnAlpha = 0.05f;
        private ParticleSystem[] m_particleSystems;

        private float _currentSpeed = 0.0f;
        
        public Transform m_Turret;

        private Vector3 _targetMoveDirection;
        private Vector3 _targetTurretDirection;
        private CharacterController _characterController;

        private void Awake ()
        {
            _characterController = GetComponent<CharacterController>();
            Application.targetFrameRate = 60;
        }
        private void OnEnable ()
        {

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable ()
        {
            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Update ()
        {
            // Store the value of both input axes.
            _targetMoveDirection = new Vector3 (Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            MoveChassis();
        }

        private void MoveChassis()
        {
            if(_targetMoveDirection.sqrMagnitude < 0.01f)
            {
                return;
            }
            float diffAngle = Vector3.Angle(transform.forward, _targetMoveDirection);
            Vector3 currentMoveDirection = Vector3.Lerp(transform.forward, _targetMoveDirection.normalized, m_TurnAlpha) * _targetMoveDirection.magnitude;
            _currentSpeed = m_Speed * (1.0f - diffAngle / 180.0f);
            transform.right = new Vector3(currentMoveDirection.x, 0.0f, currentMoveDirection.z);
            _characterController.Move(transform.TransformDirection(currentMoveDirection) * _currentSpeed * Time.deltaTime);
        }

        private void MoveTurret()
        {

        }
    }
}