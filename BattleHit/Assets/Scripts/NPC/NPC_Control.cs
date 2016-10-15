using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;
using UnityStandardAssets.CrossPlatformInput;

public class NPC_Control : MonoBehaviour
{
    public int m_iConverTextNo;

    PlayerController m_Player = null;
    FieldUI_Control fieldUI = null;

    void Start()
    {
        m_Player = FindObjectOfType<PlayerController>();
        fieldUI = UIManager.Instance().GetFieldUI() as FieldUI_Control;
    }

    void Update()
    {
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<PlayerController>();
        }

        if (m_Player == null) return;

        float vDis = Vector2.Distance(m_Player.transform.position, transform.position);
        if (vDis < 0.3f)
        {
            if (fieldUI == null)
            {
                fieldUI = UIManager.Instance().GetFieldUI() as FieldUI_Control;

                if (fieldUI == null) return;
            }

            if (m_Player.PhyCtrl.IsMoving) return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                fieldUI.StartConver(m_iConverTextNo);
            }
#else
            if (Input.touchCount > 0)
            {
                fieldUI.StartConver(m_iConverTextNo);          
            }
#endif
        }
    }
}
