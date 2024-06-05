using UnityEngine;
using UnityEngine.Rendering;

public class TankMovement : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnAlpha = 0.05f;

    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public float m_CurrentLaunchForce = 15.0f;
    private ParticleSystem[] m_particleSystems;

    private float _currentSpeed = 0.0f;

    public Transform m_Turret;

    private Vector3 _targetMoveDirection;
    private Vector3 _targetTurretDirection;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 60;
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        _targetMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000.0f))
        {
            Vector3 mousePoint = hitData.point;
            mousePoint.y = m_Turret.position.y;
            _targetTurretDirection = (mousePoint - m_Turret.position).normalized;
        }

        MoveChassis();
        MoveTurret();
    }

    private void MoveChassis()
    {
        if (_targetMoveDirection.sqrMagnitude < 0.01f)
        {
            return;
        }
        float diffAngle = Vector3.Angle(transform.forward, _targetMoveDirection);
        Vector3 currentMoveDirection = Vector3.Slerp(transform.forward, _targetMoveDirection.normalized, m_TurnAlpha);
        _currentSpeed = m_Speed * Mathf.Max(0.0f, (1.0f - diffAngle / 60.0f));
        transform.forward = currentMoveDirection;
        _characterController.Move(currentMoveDirection * _currentSpeed * Time.deltaTime + Vector3.down);
    }

    private void MoveTurret()
    {
        Vector3 currentTurretDirection = Vector3.Slerp(m_Turret.forward, _targetTurretDirection, m_TurnAlpha);
        m_Turret.forward = currentTurretDirection;
    }

    private void Fire()
    {
        Debug.Break();
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * Random.Range(0.8f, 1.2f) * m_FireTransform.forward;
    }
}