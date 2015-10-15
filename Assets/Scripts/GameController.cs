using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text m_enemyKilledCounter;

    private int m_totalEnemyKilled;

    private string enemyKilled {
        set {
            if (m_enemyKilledCounter) {
                m_enemyKilledCounter.text = value;
            }
        }
    }

    public void killAnEnemy() { 
        m_totalEnemyKilled++;
        enemyKilled = "Total Enemies Killed: " + m_totalEnemyKilled.ToString();
    }

    void Start() {
        m_totalEnemyKilled = 0;
    }
}
