using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : UnitController {
    public float m_fuel;
    public float m_fuelRefillFreq;
    public float m_dashCost;
    public Image m_healthBar;
    public Image m_fuelBar;

    private const float TURN_SPEED = 20f;
    private const int FILE_MOUSE_BUTTON_ID = 0;
    private const int AIM_MOUSE_BUTTON_ID = 1;

    private float m_curFuel;
    private float m_curFuelRefillTime;
    private bool m_restoringHealth;
    private bool m_healthEffectAlternate;

    private float healthAmount {
        set {
            if (m_healthBar) {
                m_healthBar.fillAmount = value;
            }
        }
    }

    private float fuelAmount {
        set {
            if (m_fuelBar) {
                m_fuelBar.fillAmount = value;
            }
        }
    }

    private Color healthBarColor {
        get {
            if (m_healthBar) { return m_healthBar.color; }
            return Color.white;
        }

        set {
            if (m_healthBar) {
                m_healthBar.color = value;
            }
        }
    }

    public override void damage(int amount) {
        base.damage(amount);
        healthAmount = m_curHealth / m_health;
    }

    public override void restoreHealth(int amount) {
        base.restoreHealth(amount);
        healthAmount = m_curHealth / m_health;
        m_restoringHealth = true;
    }

    public void leaveRegion() {
        m_restoringHealth = false;
        m_healthEffectAlternate = true;
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

    protected override void Start() {
        base.Start();
        m_fuelRefillFreq = Mathf.Max(0, m_fuelRefillFreq);
        m_curFuelRefillTime = 0;
        m_fuel = Mathf.Max(0, m_fuel);
        m_curFuel = m_fuel;
        m_dashCost = Mathf.Max(0, m_dashCost);
        m_restoringHealth = false;
        m_healthEffectAlternate = true;
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

        if (m_restoringHealth) {
            if (m_healthEffectAlternate) {
                healthBarColor = Color.Lerp(healthBarColor, Color.green, 5 * Time.deltaTime);
                if (healthBarColor == Color.green) { m_healthEffectAlternate = false; }
            } else {
                healthBarColor = Color.Lerp(healthBarColor, Color.white, 5 * Time.deltaTime);
                if (healthBarColor == Color.white) { m_healthEffectAlternate = true; }
            }
        } else {
            healthBarColor = Color.white;
        }

        m_curFuelRefillTime += Time.deltaTime;
        if (m_curFuelRefillTime >= m_fuelRefillFreq) {
            m_curFuelRefillTime = 0;
            m_curFuel += 0.1f;
            m_curFuel = Mathf.Min(m_curFuel, m_fuel);
            fuelAmount = m_curFuel / m_fuel;
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
            if (m_curFuel - m_dashCost >= 0) {
                m_curFuel -= m_dashCost;
                fuelAmount = m_curFuel / m_fuel;
                move(moveDir, 10 * m_curMoveSpeed);
            }
        }
    }
}
