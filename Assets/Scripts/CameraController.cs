using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraController : CameraPivot {
    private const int AIM_BUTTON_ID = 1;
    private const float LOOK_DISTANCE = 100f;
    private const float AIM_DISTANCE = 2f;
    private const float AIM_SPEED = 10f;

    public float m_turnSpeed;
    public float m_turnSmoothing;
    public float m_tiltMax;
    public float m_tiltMin;

    private Vector3 m_camInitalState;
    private float m_moveSpeed;
    private float m_lookAngle;
    private float m_tiltAngle;
    private float m_smoothX;
    private float m_smoothY;
    private float m_smoothXVelocity;
    private float m_smoothYVelocity;
    private float m_aimingWeight;
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
        m_camInitalState = m_camera.transform.localPosition;

        if (m_target) {
            UnitController player = m_target.GetComponent<UnitController>();
            m_moveSpeed = player.m_moveSpeed * 10;
        }
    }

    protected override void Update() {
        base.Update();

        if (Application.isPlaying) {
            if (Input.GetKeyUp(KeyCode.LeftControl)) {
                m_lockCursor = !m_lockCursor;
            }

            if (m_lockCursor) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                rotateCamera();
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        bool aimed = Input.GetMouseButton(AIM_BUTTON_ID);
        m_aimingWeight = Mathf.MoveTowards(m_aimingWeight, aimed ? 1f : 0f, AIM_SPEED * Time.deltaTime);

        Vector3 aimingState = m_camInitalState + new Vector3(0f, 0f, AIM_DISTANCE);
        m_camera.transform.localPosition = Vector3.Lerp(m_camInitalState, aimingState, m_aimingWeight);

    }

    protected override void follow(float deltaTime) {
        if (target) {
            transform.position = Vector3.Lerp(transform.position, target.position, m_moveSpeed * deltaTime);
        }
    }

    private void rotateCamera() {
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
