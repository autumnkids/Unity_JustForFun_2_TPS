using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
    public enum UserType { PLAYER, ENEMY }

    public UserType m_userType;
    public GameObject m_bulletPref;
    public LayerMask m_canHitMask;
    public int m_attack;
    public float m_attackRange;
    public float m_instability;

    private GameObject m_playerObj;
    private float m_curInstability;

    public void shoot() {
        Vector3 genPos = transform.TransformPoint(Vector3.zero);
        float instability = Random.Range(-m_instability, m_instability);
        genPos += instability * Vector3.one;
        GameObject bulletObj = Instantiate(m_bulletPref, genPos, Quaternion.identity) as GameObject;
        BulletController bullet = bulletObj.GetComponent<BulletController>();
        bullet.parent = transform;
        bullet.moveDir = transform.forward;
        bullet.longestDistance = m_attackRange;
        bullet.attack = m_attack;
        bullet.canHitMask = m_canHitMask;
        if (m_userType == UserType.PLAYER) { bullet.namedTag = Tags.PLAYER; }
        else if (m_userType == UserType.ENEMY) { bullet.namedTag = Tags.ENEMY; }
    }

    public void aiming(bool aimed) {
        if (aimed) { m_curInstability = 0; }
        else { m_curInstability = m_instability; }
    }

    void Start() {
        m_playerObj = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        m_curInstability = m_instability;
    }

    void Update() {
        Vector3 lookPos = Vector3.zero;
        if (m_userType == UserType.PLAYER) {
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            lookPos = cameraRay.GetPoint(m_attackRange);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, m_attackRange, m_canHitMask)) {
                lookPos = hit.point;
            }
        } else if (m_userType == UserType.ENEMY) {
            lookPos = m_playerObj.transform.position;
        }
        transform.forward = lookPos - transform.position;
    }
}
