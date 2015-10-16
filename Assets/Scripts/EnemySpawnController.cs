using UnityEngine;
using System.Collections;

public class EnemySpawnController : MonoBehaviour {
    public bool m_triggerOnAwake;
    public GameObject[] m_enemyPrefs;
    public int m_maxGenAmount;
    public float m_genGap;
    public float m_sqaureWidth;

    private int m_curAmount;
    private float m_curTime;
    private bool m_canTrigger;

    public void onClick_Spawn() {
        m_canTrigger = true;
        if (m_curAmount >= m_maxGenAmount) { m_curAmount = 0; }
    }

    void Start() {
        m_curTime = 0f;
        m_curAmount = 0;
        if (m_triggerOnAwake) { m_canTrigger = true; }
        else { m_canTrigger = false; }
    }

    void Update() {
        if (m_canTrigger) {
            m_curTime += Time.deltaTime;

            if (m_curTime >= m_genGap && m_curAmount < m_maxGenAmount) {
                m_curTime = 0;
                float offset = Random.Range(-m_sqaureWidth / 2f, m_sqaureWidth / 2f + 1);
                float x = transform.position.x + offset;
                float z = transform.position.z + offset;

                int enemySize = m_enemyPrefs.Length;
                int genIndex = Random.Range(0, enemySize);
                GameObject genObj = Instantiate(m_enemyPrefs[genIndex], new Vector3(x, 0.6f, z), Quaternion.identity) as GameObject;
                genObj.transform.SetParent(transform);
                m_curAmount++;
            } else if (m_curAmount >= m_maxGenAmount) {
                m_canTrigger = false;
            }
        }
    }
}
