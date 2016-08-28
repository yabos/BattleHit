using UnityEngine;
using System.Collections;
 
public class FadeInOutManager : MonoBehaviour
{
    public SpriteRenderer m_FadeImage = null;

    public bool m_bFadeIn = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (m_bFadeIn)
        {
            FadeIn();
        }
    }

    public void StartFadeIn()
    {
        m_bFadeIn = true;
        m_FadeImage.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void FadeIn()
    {
        if (m_FadeImage == null) return;

        if(!m_FadeImage.gameObject.activeSelf)
            m_FadeImage.gameObject.SetActive(true);

        SetFadeImagePos();

        Color color = m_FadeImage.GetComponent<SpriteRenderer>().color;
        m_FadeImage.GetComponent<SpriteRenderer>().color = Color.Lerp(color, Color.clear, Time.deltaTime);

        if (color.Equals(Color.clear))
        {
            m_bFadeIn = false;
        }
    }

    void FadeOut()
    {
        if (m_FadeImage == null) return;

        if (!m_FadeImage.gameObject.activeSelf)
            m_FadeImage.gameObject.SetActive(true);

        SetFadeImagePos();

        Color color = m_FadeImage.GetComponent<SpriteRenderer>().color;
        m_FadeImage.GetComponent<SpriteRenderer>().color = Color.Lerp(color, Color.white, Time.deltaTime);
    }

    //public void FadeInOut()
    //{
    //    if (m_FadeImage == null) return;

    //    SetFadeImagePos();

    //    Color color = m_FadeImage.GetComponent<SpriteRenderer>().color;
    //    m_FadeImage.GetComponent<SpriteRenderer>().color = Color.Lerp(color, Color.clear, Time.deltaTime);

    //    Color color = m_FadeImage.GetComponent<SpriteRenderer>().color;
    //    m_FadeImage.GetComponent<SpriteRenderer>().color = Color.Lerp(color, Color.white, Time.deltaTime);
    //}

    void SetFadeImagePos()
    {
        Vector3 vCamPos = Camera.main.transform.position;
        m_FadeImage.transform.position = new Vector3(vCamPos.x, vCamPos.y, 0);
    }
}