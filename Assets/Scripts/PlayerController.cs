using UnityEngine;
using System.Collections;

public class PlayerController : UnitController {
    void FixedUpdate() {
        Vector3 moveDir = Vector3.zero;

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

        if (moveDir != Vector3.zero) {
            moveDir.Normalize();
            m_state = UnitState.MOVING;
        } else {
            m_state = UnitState.IDLING;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(m_rigidbody.velocity.y) < 0.0001f) {
            m_state = UnitState.JUMPING;
        }
        
        if (m_state == UnitState.MOVING) {
            Vector3 moveTargetPos = transform.position + moveDir * m_moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, moveTargetPos, m_moveSpeed * Time.deltaTime);
        } else if (m_state == UnitState.JUMPING) {
            m_rigidbody.AddForce(Vector3.up * m_jumpSpeed);
        }
    }
}
