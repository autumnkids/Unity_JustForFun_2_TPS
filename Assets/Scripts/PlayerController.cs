using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : UnitController {
    public Image m_healthBar;

    private const float TURN_SPEED = 20f;
    private const int FILE_MOUSE_BUTTON_ID = 0;
    private const int AIM_MOUSE_BUTTON_ID = 1;

    private float fillAmount {
        set {
            if (m_healthBar) {
                m_healthBar.fillAmount = value;
            }
        }
    }

    public override void damage(int amount) {
        base.damage(amount);
        fillAmount = m_curHealth / m_health;
    }

    void move(Vector3 moveDir, float moveSpeed) {
        Vector3 moveTargetPos = transform.position + moveDir * moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, moveTargetPos, moveSpeed * Time.deltaTime);
    }

    void rotateByCamera() {
        transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
    }

    void aiming(bool aimed) {
        m_weapon.aiming(aimed);
    }

    void Update() {
        rotateByCamera();

        if (Input.GetMouseButtonDown(0)) {
            m_weapon.shoot();
        }
        if (Input.GetMouseButtonDown(1)) {
            aiming(true);
            m_curMoveSpeed = m_moveSpeed * 0.2f;
        } else if (Input.GetMouseButtonUp(1)) {
            aiming(false);
            m_curMoveSpeed = m_moveSpeed;
        }
    }

    void FixedUpdate() {
        Vector3 moveDir = Vector3.zero;

        // Set movement directions
        if (Input.GetKey(KeyCode.W)) {
            moveDir += transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDir += -transform.right;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDir += -transform.forward;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDir += transform.right;
        }

        // Judge moving states
        if (moveDir != Vector3.zero) {
            moveDir.Normalize();
            m_state = UnitState.MOVING;
        } else {
            m_state = UnitState.IDLING;
        }

        // Judge jump states
        if (!m_onTheGround && Mathf.Abs(m_rigidbody.velocity.y) < 0.001f) {
            m_onTheGround = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_onTheGround) {
            m_state = UnitState.JUMPING;
        }

        if (m_state == UnitState.MOVING && m_onTheGround) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                m_state = UnitState.DASH;
            }
        }
        
        if (m_state == UnitState.MOVING) {
            move(moveDir, m_curMoveSpeed);
        } else if (m_state == UnitState.JUMPING) {
            m_onTheGround = false;
            m_rigidbody.AddForce(Vector3.up * m_jumpSpeed);
        } else if (m_state == UnitState.DASH) {
            move(moveDir, 10 * m_curMoveSpeed);
        }
    }
}
