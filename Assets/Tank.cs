using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] TankMovement _movement;
    [SerializeField] TurretComponent _turret;

    void Start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _turret.Fire();
        }   

         _movement.SetMoveInput(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000.0f))
        {
            Vector3 mousePoint = hitData.point;
            mousePoint.y = _turret.transform.position.y;
            _turret.SetLookDirection((mousePoint - _turret.transform.position).normalized);
        }
    }
}
