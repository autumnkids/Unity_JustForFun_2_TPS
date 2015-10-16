using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : UnitController {
    public Canvas m_uiCanvas;
    public Image m_healthBar;
    public float m_shootFrequency;

    private GameController m_gameController;
    private NavMeshAgent m_agent;
    private GameObject m_playerObj;
    private float m_curTime;
    private bool m_readyToShoot;

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
        if (m_curHealth <= 0) {
            m_gameController.killAnEnemy();
            Destroy(gameObject); 
        }
    }

    void Awake() {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_moveSpeed;
        m_agent.stoppingDistance = m_weapon.m_attackRange * 0.5f;
    }

    protected override void Start() {
        base.Start();
        m_curTime = 0f;
        m_readyToShoot = false;
        m_playerObj = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        m_gameController = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameController>();
    }

    void Update() {
        m_curTime += Time.deltaTime;
        if (m_curTime >= m_shootFrequency) {
            m_curTime = m_shootFrequency;
            m_readyToShoot = true;
        }

        if (Vector3.Distance(m_playerObj.transform.position, transform.position) <= m_weapon.m_attackRange && m_readyToShoot) {
            m_readyToShoot = false;
            m_curTime = 0f;
            m_weapon.shoot();
        }

        transform.forward = Vector3.Slerp(transform.forward, m_playerObj.transform.position - transform.position, Time.deltaTime);
        m_uiCanvas.transform.forward = Camera.main.transform.position - m_uiCanvas.transform.position;
    }

    void FixedUpdate() {
        print(transform.position.y);
        if (!m_onTheGround && Mathf.Abs(transform.position.y) < 0.6f) {
            m_onTheGround = true;
            m_agent.updatePosition = true;
        }

        if (m_onTheGround) {
            m_agent.SetDestination(m_playerObj.transform.position);
        }
        m_state = UnitState.MOVING;

        Vector3 playerPos = m_playerObj.transform.position;
        if (playerPos.y > 1f && m_onTheGround) {
            m_state = UnitState.JUMPING;
        } else {
            m_state = UnitState.MOVING;
        }

        if (m_state == UnitState.JUMPING) {
            m_onTheGround = false;
            m_agent.updatePosition = false;
            m_rigidbody.AddForce(Vector3.up * m_jumpSpeed);
        }
    }
}
