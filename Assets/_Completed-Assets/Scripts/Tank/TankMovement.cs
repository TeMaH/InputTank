using UnityEngine;
using UnityEngine.Rendering;

public class TankMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float TurnAlpha = 0.05f;
    
    private ParticleSystem[] m_particleSystems;

    private float _currentSpeed = 0.0f;


    private Vector3 _targetMoveDirection;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
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


    private void OnDisable()
    {
        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
    }

    public void SetMoveInput(Vector3 input)
    {
        _targetMoveDirection = input;
    }

    private void Update()
    {
       if (_targetMoveDirection.sqrMagnitude < 0.01f)
        {
            return;
        }
        float diffAngle = Vector3.Angle(transform.forward, _targetMoveDirection);
        Vector3 currentMoveDirection = Vector3.Slerp(transform.forward, _targetMoveDirection.normalized, TurnAlpha);
        _currentSpeed = Speed * Mathf.Max(0.0f, (1.0f - diffAngle / 60.0f));
        transform.forward = currentMoveDirection;
        _characterController.Move(currentMoveDirection * _currentSpeed * Time.deltaTime + Vector3.down);
    }
}