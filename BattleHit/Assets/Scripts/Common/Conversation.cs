using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CreativeSpore.RpgMapEditor;
using UnityStandardAssets.CrossPlatformInput;

public class Conversation : MonoBehaviour
{
    public UILabel m_LabelName;
    public UILabel m_LabelText;
    public UISprite m_SpriteContinue;

    List<string> m_ListConver = new List<string>();
    int m_iCurConverStep = 1;
    bool m_bConverIng = false;
    bool m_bTweenScaleEvent = false;
    
    PlayerController m_Player = null;

    FieldUI_Control.CallBackConverEnd CallBack = null;

    public void InitConver(int iNPCNo, FieldUI_Control.CallBackConverEnd Call)
    {
        if (m_bConverIng) return;

        m_ListConver.Clear();

        string st = TBManager.Instance().GetConverText(iNPCNo);
        string [] stText = st.Split('_');
        if (stText == null) return;

        m_LabelName.text = stText[0];
        m_ListConver.AddRange(stText);

        CallBack = Call;
        ConverText();

        TweenScale ts = GetComponent<TweenScale>();
        if (ts != null)
        {
            ts.ResetToBeginning();
            ts.from = new Vector3(0.1f, 0.1f, 0.1f);
            ts.to = Vector3.one;
            ts.duration = 0.1f;
            ts.PlayForward();

            m_bTweenScaleEvent = true;
        }
    }

    void ConverText()
    {
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<PlayerController>();
            if (m_Player == null) return;
        }

        if (m_iCurConverStep < m_ListConver.Count)
        {
            m_LabelText.text = m_ListConver[m_iCurConverStep];
            ++m_iCurConverStep;
            m_bConverIng = true;
            m_Player.PhyCtrl.IsStop = true;
            if (m_iCurConverStep == m_ListConver.Count)
            {
                m_SpriteContinue.gameObject.SetActive(false);
            }
            else
            {
                m_SpriteContinue.gameObject.SetActive(true);
            }            
        }
        else
        {
            TweenScale ts = GetComponent<TweenScale>();
            if (ts != null)
            {
                ts.ResetToBeginning();
                ts.from = Vector3.one;
                ts.to = new Vector3(0.1f, 0.1f, 0.1f);
                ts.duration = 0.1f;
                ts.PlayForward();

                m_bTweenScaleEvent = false;
            }
        }
    }

    // Update is called once per frame    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            ConverText();
        }
#else
        if (Input.touchCount > 0)
        {
            ConverText();
        }
#endif
    }

    public void EndTweenScale()
    {
        if (m_bTweenScaleEvent) return;

        if (m_Player != null && m_Player.PhyCtrl != null)
        {
            m_Player.PhyCtrl.IsStop = false;
        }
        m_bConverIng = false;
        m_iCurConverStep = 1;
        CallBack();
    }
}
