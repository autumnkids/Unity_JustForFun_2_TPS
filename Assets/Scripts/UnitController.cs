using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {
    public enum UnitState { 
        MOVING,
        IDLING,
        JUMPING,
        DASH
    }

    public WeaponController m_weapon;
    public int m_health;
    public float m_moveSpeed;
    public float m_jumpSpeed;

    protected UnitState m_state;
    protected Rigidbody m_rigidbody;
    protected bool m_onTheGround;
    protected float m_curMoveSpeed;
    protected float m_curHealth;

    public virtual void damage(int amount) {
        m_curHealth -= amount;
        m_curHealth = Mathf.Max(0, m_curHealth);
    }

    public virtual void restoreHealth(int amount) {
        m_curHealth += amount;
        m_curHealth = Mathf.Min(m_curHealth, m_health);
    }

    protected virtual void Start() {
        m_state = UnitState.IDLING;
        m_onTheGround = true;
        m_curMoveSpeed = m_moveSpeed;
        m_health = Mathf.Max(0, m_health);
        m_curHealth = m_health;
        m_moveSpeed = Mathf.Max(0, m_moveSpeed);
        m_jumpSpeed = Mathf.Max(0, m_jumpSpeed);
        m_rigidbody = GetComponent<Rigidbody>();
    }
}
