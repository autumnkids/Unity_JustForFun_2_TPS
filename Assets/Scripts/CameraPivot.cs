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
        // Running in edit mode, so that the camera can be attached to the player
        if (!Application.isPlaying) {
            if (target != null) {
                follow(999);
                m_lastTargetPos = target.position;
            }

            m_camera.localPosition = Vector3.Scale(m_camera.localPosition, Vector3.forward);
        }
    }

    protected override void follow(float deltaTime) { }
}
