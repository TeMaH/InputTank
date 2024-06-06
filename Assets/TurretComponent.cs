using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretComponent : MonoBehaviour
{
    [SerializeField] Rigidbody _shell;
    [SerializeField] Transform _shellPivot;
    [SerializeField] float _launchForce = 15.0f;
    [SerializeField] float _turnAlpha = 0.05f;

    private Vector3 _targetTurretDirection;

    public void SetLookDirection(Vector3 direction)
    {
        _targetTurretDirection = direction.normalized;
    }
    
    void Update()
    {
        Vector3 currentTurretDirection = Vector3.Slerp(transform.forward, _targetTurretDirection, _turnAlpha);
        transform.forward = currentTurretDirection;
    }

    public void Fire()
    {
        var shellInstance = Instantiate(_shell, _shellPivot.position, _shellPivot.rotation);
        shellInstance.velocity = _launchForce * Random.Range(0.5f, 1.5f) * _shellPivot.forward;
    }
}
