using UnityEngine;
using System.Collections;

public enum eFieldUIState
{
    eFieldUI_Controller,
    eFieldUI_Conver,
}

public class FieldUI_Control : BaseUI
{
    const float CONVER_DELAY = 1f;

    Transform mConver = null;
    Conversation mUIConver = null;
    float m_fElapsedTime = CONVER_DELAY;

    public delegate void CallBackConverEnd();

    // Use this for initialization
    void Start ()
    {
        mConver = transform.FindChild("Anchor_T/Conver");
        if (mConver == null) return;

        mUIConver = mConver.GetComponent<Conversation>();
    }

    void Update()
    {
        m_fElapsedTime += Time.deltaTime;
    }

    public void ActiveFieldUI(eFieldUIState state)
    {
        mConver.gameObject.SetActive(state == eFieldUIState.eFieldUI_Conver);
    }

    public void StartConver(int iNPCNo)
    {
        if (m_fElapsedTime < CONVER_DELAY) return;

        if (mUIConver != null)
        {
            mUIConver.InitConver(iNPCNo, ConverEnd);
            ActiveFieldUI(eFieldUIState.eFieldUI_Conver);
        }
    }

    void ConverEnd()
    {
        mConver.gameObject.SetActive(false);
        m_fElapsedTime = 0;
    }
}
