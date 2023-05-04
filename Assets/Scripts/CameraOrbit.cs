using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraOrbit : MonoBehaviour
{
    public Transform follow;
    public float maxDistance;
    public Vector2 sensitivity;

    private Vector2 angle = new Vector2(-90 * Mathf.Deg2Rad, 0);
    private Camera cam;
    private Vector2 nearPlaneSice;

    void Start()
    {
        //Puede que se necesite un método al bloquear el cursor para poder acceder al inventario y otra parafernalia.
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
        CalculateNearPlaneSice();
    }

    private void Update()
    {
        float hor = Input.GetAxis("Mouse X");

        if (hor != 0)
            angle.x += hor * Mathf.Deg2Rad * sensitivity.x * Time.deltaTime;

        float ver = Input.GetAxis("Mouse Y");

        if(ver != 0)
        {
            angle.y += ver * Mathf.Deg2Rad * sensitivity.y * Time.deltaTime;
            angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }
    }

    void LateUpdate()
    {
        Vector3 orbit = new Vector3(Mathf.Cos(angle.x) * Mathf.Cos(angle.y), -Mathf.Sin(angle.y), -Mathf.Sin(angle.x) * Mathf.Cos(angle.y));

        RaycastHit hit;
        float distance = maxDistance;

        Vector3[] points = GetCameraCollisionPoints(orbit);

        foreach (Vector3 point in points)
        {
            if(Physics.Raycast(follow.position, orbit, out hit, maxDistance))
            {
                distance = Mathf.Min((hit.point - follow.position).magnitude, distance);
            }
        }

        transform.position = follow.position + orbit * distance;
        transform.rotation = Quaternion.LookRotation(follow.position -transform.position);
    }

    private void CalculateNearPlaneSice()
    {
        float height = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2) * cam.nearClipPlane;
        float width = height * cam.aspect;

        nearPlaneSice = new Vector2(width, height);
    }

    Vector3[] GetCameraCollisionPoints(Vector3 orbit)
    {
        Vector3 position = follow.position;
        Vector3 center = position + orbit * (cam.nearClipPlane + 0.1f);

        Vector3 right = transform.right * nearPlaneSice.x;
        Vector3 up = transform.up * nearPlaneSice.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }
}
