using UnityEngine;
using System.Collections;

public abstract class FollowTarget : MonoBehaviour {
    protected GameObject m_target;

    public Transform target {
        get {
            if (m_target) {
                return m_target.transform;
            }
            return null;
        }
    }

    /// <summary>
    /// Camera following function, with the specific movement delta time
    /// </summary>
    /// <param name="deltaTime"></param>
    protected abstract void follow(float deltaTime);

    protected void findTargetPlayer() {
        if (m_target == null) {
            m_target = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        }
    }

    protected virtual void Start() {
        findTargetPlayer();
    }

    protected virtual void LateUpdate() {
        if (m_target == null) { findTargetPlayer(); }
        
        follow(Time.deltaTime);
    }
}
