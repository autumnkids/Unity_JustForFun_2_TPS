using UnityEngine;
using System.Collections;

public class BuffRegion : RegionTrigger {
    public int m_regionBuffAmount;
    public float m_triggerGap;
    public LayerMask m_characterLayer;

    private Collider m_collider;
    private float m_curTime;
    private bool m_timeToTrigger;

    protected override void Start() {
        base.Start();
        m_curTime = 0;
        m_timeToTrigger = false;
        m_collider = GetComponent<Collider>();
    }

    void Update() {
        m_curTime += Time.deltaTime;

        if (m_curTime >= m_triggerGap) {
            m_curTime = 0;
            m_timeToTrigger = true;
        }
    }

    void FixedUpdate() {
        if (m_timeToTrigger) {
            m_timeToTrigger = false;
            Collider[] collisions = Physics.OverlapSphere(transform.position, m_collider.bounds.size.x / 2f, m_characterLayer);
            if (collisions.Length > 0) {
                foreach (Collider obj in collisions) {
                    if (obj.tag == Tags.PLAYER || obj.tag == Tags.ENEMY) { 
                        UnitController character = obj.gameObject.GetComponent<UnitController>();
                        if (m_type == RegionTriggerType.DAMAGE) {
                            character.damage(m_regionBuffAmount);
                        } else if (m_type == RegionTriggerType.RESTORE) {
                            character.restoreHealth(m_regionBuffAmount);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == Tags.PLAYER) {
            other.gameObject.SendMessage("leaveRegion");
        }
    }
}
