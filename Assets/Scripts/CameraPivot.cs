using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraPivot : FollowTarget {
    protected Transform m_camera;
    protected Transform m_pivot;
    protected Vector3 m_lastTargetPos;

    protected override void Start() {
        base.Start();

        m_camera = Camera.main.transform;
        m_pivot = m_camera.parent;
    }

    protected virtual void Update() {
        if (!Application.isPlaying) {
            if (m_target != null) {
                follow(Time.deltaTime);
                m_lastTargetPos = m_target.position;
            }

            if (Mathf.Abs(m_camera.localPosition.x) > 0.5f || Mathf.Abs(m_camera.localPosition.y) > 0.5f) {
                m_camera.localPosition = Vector3.Scale(m_camera.localPosition, Vector3.forward);
            }

            m_camera.localPosition = Vector3.Scale(m_camera.localPosition, Vector3.forward);
        }
    }

    protected override void follow(float deltaTime) { }
}
