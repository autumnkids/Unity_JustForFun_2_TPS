using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class RegionTrigger : MonoBehaviour {
    public enum RegionTriggerType { SHOW_TEXT, DAMAGE, RESTORE }

    public RegionTriggerType m_type;
    public bool m_hasTriggerText;
    public string m_regionName;

    private GameObject m_mainTitleTextObj;

    protected virtual void Start() {
        m_mainTitleTextObj = GameObject.FindGameObjectWithTag(Tags.MAIN_TEXT_TITLE);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (m_mainTitleTextObj && m_hasTriggerText && other.tag == Tags.PLAYER) {
            TriggerText triggerText = m_mainTitleTextObj.GetComponent<TriggerText>();
            if (triggerText) {
                triggerText.showText(m_regionName);
            }
        }
    }
}
