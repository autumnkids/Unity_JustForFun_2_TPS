using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {
    public enum UnitState { 
        MOVING,
        IDLING,
        JUMPING
    }

    public float m_moveSpeed;
    public float m_jumpSpeed;

    protected UnitState m_state;
    protected Rigidbody m_rigidbody;
    protected bool m_onTheGround;

    void Start() {
        m_state = UnitState.IDLING;
        m_onTheGround = true;
        m_rigidbody = GetComponent<Rigidbody>();
    }
}
