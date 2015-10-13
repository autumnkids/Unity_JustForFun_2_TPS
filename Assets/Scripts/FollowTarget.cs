using UnityEngine;
using System.Collections;

public abstract class FollowTarget : MonoBehaviour {
    protected Transform m_target;

    public Transform target {
        get { return m_target; }
    }

    protected abstract void follow(float deltaTime);

    protected void findTargetPlayer() {
        if (m_target == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag(Tags.PLAYER);
            if (playerObj) {
                setTarget(playerObj.transform);
            }
        }
    }

    protected virtual void setTarget(Transform target) {
        m_target = target;
    }

    protected virtual void Start() {
        findTargetPlayer();
    }

    void FixedUpdate() {
        if (m_target == null) { findTargetPlayer(); }

        follow(Time.deltaTime);
    }
}
