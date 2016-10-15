using UnityEngine;
using System.Collections;

public class HeroDamage : MonoBehaviour
{
    public UILabel m_LabelDamage;

    Rigidbody2D m_Rigid2d = null;
    public bool m_bMyTeam = true;

    void Start()
    {
        Rigidbody2D rigid = m_LabelDamage.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            m_Rigid2d = rigid;
            if (m_bMyTeam)
            {
                m_Rigid2d.AddForce(new Vector2(-0.005f, 0.005f));
            }
            else
            {
                m_Rigid2d.AddForce(new Vector2(0.005f, 0.005f));
            }
        }
    }

    //public void SetForce(bool bMyTeam)
    //{
    //    if (m_Rigid2d != null)
    //    {
    //        if (bMyTeam)
    //        {
    //            m_Rigid2d.AddForce(new Vector2(-0.01f, 0.005f));
    //        }
    //        else
    //        {
    //            m_Rigid2d.AddForce(new Vector2(0.01f, 0.005f));
    //        }
    //    }
    //}
}
