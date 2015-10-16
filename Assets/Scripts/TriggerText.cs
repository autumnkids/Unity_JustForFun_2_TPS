using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TriggerText : MonoBehaviour {
    public enum TriggerTextState { INVISIBLE, VISIBLE, FADE_IN, FADE_OUT }

    public float m_showDuration;
    public float m_fadeSpeed;

    private Text m_text;
    private TriggerTextState m_state;
    private bool m_timeToFadeIn;
    private float m_showTime;

    private string text {
        set {
            if (m_text) {
                m_text.text = value;
            }
        }
    }

    private float alpha {
        get {
            if (m_text) {
                return m_text.color.a;
            }
            return 1f;
        }
    }

    private Color alpha_opaque100 {
        get { 
            if (m_text) {
                return new Color(m_text.color.r, m_text.color.g, m_text.color.b, 1f);
            }
            return Color.white;
        }
    }

    private Color alpha_opaque0 {
        get {
            if (m_text) {
                return new Color(m_text.color.r, m_text.color.g, m_text.color.b, 0f);
            }
            return Color.white;
        }
    }

    private Color color {
        get {
            if (m_text) { return m_text.color; }
            return Color.white;
        }

        set {
            if (m_text) { m_text.color = value; }
        }
    }

    public void showText(string t) {
        text = t;
        m_state = TriggerTextState.FADE_IN;
    }

    void fadeIn() {
        color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 1, m_fadeSpeed * Time.deltaTime));
    }

    void fadeOut() {
        color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, m_fadeSpeed * Time.deltaTime));
    }

    void Start() {
        m_text = gameObject.GetComponent<Text>();
        m_state = TriggerTextState.INVISIBLE;
        m_showTime = 0;
        color = alpha_opaque0;
    }

    void Update() {
        if (m_state == TriggerTextState.VISIBLE) {
            m_showTime += Time.deltaTime;
            if (m_showTime >= m_showDuration) {
                m_showTime = 0;
                m_state = TriggerTextState.FADE_OUT;
            }
        } else if (m_state == TriggerTextState.FADE_IN) {
            fadeIn();
            if (alpha >= 0.99f) {
                color = alpha_opaque100;
                m_state = TriggerTextState.VISIBLE;
            }
        } else if (m_state == TriggerTextState.FADE_OUT) {
            fadeOut();
            if (alpha <= 0.01f) {
                color = alpha_opaque0;
                m_state = TriggerTextState.INVISIBLE;
            }
        }
    }
}
