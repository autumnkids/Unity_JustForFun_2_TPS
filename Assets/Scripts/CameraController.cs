using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraController : CameraPivot {
    private static float LOOK_DISTANCE = 100f;

    public float m_moveSpeed;
    public float m_turnSpeed;
    public float m_turnSmoothing;
    public float m_tiltMax;
    public float m_tiltMin;

    private float m_lookAngle;
    private float m_tiltAngle;
    private float m_smoothX;
    private float m_smoothY;
    private float m_smoothXVelocity;
    private float m_smoothYVelocity;
    private bool m_lockCursor;

    protected override void Start() {
        base.Start();

        m_lookAngle = 0f;
        m_tiltAngle = 0f;
        m_smoothX = 0f;
        m_smoothY = 0f;
        m_smoothXVelocity = 0f;
        m_smoothYVelocity = 0f;
        m_lockCursor = true;
    }

    protected override void Update() {
        base.Update();

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            m_lockCursor = !m_lockCursor;
        }

        if (m_lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            moveCamera();
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    protected override void follow(float deltaTime) {
        transform.position = Vector3.Lerp(transform.position, m_target.position, m_moveSpeed * deltaTime);
    }

    private void moveCamera() {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (m_turnSmoothing > 0) {
            m_smoothX = Mathf.SmoothDamp(m_smoothX, x, ref m_smoothXVelocity, m_turnSmoothing);
            m_smoothY = Mathf.SmoothDamp(m_smoothY, y, ref m_smoothYVelocity, m_turnSmoothing);
        } else {
            m_smoothX = x;
            m_smoothY = y;
        }

        m_lookAngle += m_smoothX * m_turnSpeed;
        transform.rotation = Quaternion.Euler(0f, m_lookAngle, 0f);

        m_tiltAngle -= m_smoothY * m_turnSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_tiltMin, m_tiltMax);
        m_pivot.localRotation = Quaternion.Euler(m_tiltAngle, 0f, 0f);
    }
}
