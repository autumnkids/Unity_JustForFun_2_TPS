using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour {
    private const float BULLET_MOVE_SPEED = 100f;

    private LayerMask m_canHitMask;
    private Transform m_parent;
    private SphereCollider m_collider;
    private Rigidbody m_rigidbody;
    private Vector3 m_moveDir;
    private string m_tag;
    private float m_longestDistance;
    private int m_attack;

    public LayerMask canHitMask {
        get { return m_canHitMask; }
        set { m_canHitMask = value; }
    }

    public Transform parent {
        get { return m_parent; }
        set { m_parent = value; }
    }

    public Vector3 moveDir {
        get { return m_moveDir; }
        set { m_moveDir = value; transform.forward = m_moveDir; }
    }

    public float longestDistance {
        get { return m_longestDistance; }
        set { m_longestDistance = value; }
    }

    public int attack {
        get { return m_attack; }
        set { m_attack = value; }
    }

    public string namedTag {
        get { return m_tag; }
        set { m_tag = value; }
    }

    void FixedUpdate() {
        transform.position += m_moveDir * BULLET_MOVE_SPEED * Time.deltaTime;

        Collider[] contacts = Physics.OverlapSphere(transform.position, m_collider.radius, m_canHitMask);
        if (contacts.Length > 0) {
            Collider contact = contacts[0];
            if (m_tag != contact.tag) {
                if (contact.tag == Tags.PLAYER || contact.tag == Tags.ENEMY) {
                    UnitController unit = contact.gameObject.GetComponent<UnitController>();
                    unit.damage(unit.m_weapon.m_attack);
                }
                Destroy(gameObject);
            }
        }

        if (m_parent == null || Vector3.Distance(transform.position, m_parent.position) >= m_longestDistance) {
            Destroy(gameObject);
        }
    }

    void Awake() {
        m_moveDir = Vector3.zero;
        m_longestDistance = 100f;
        m_tag = "";
    }

    void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<SphereCollider>();
    }
}
