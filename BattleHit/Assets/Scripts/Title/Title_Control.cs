using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Title_Control : MonoBehaviour 
{
	public enum eSceneName
	{
		Scene_Title,
		Scene_Village_01,
		Scene_Dun_01,
	}

	private AsyncOperation m_Asyn = null;

    public UISprite m_SpriteProg = null;
	public UILabel m_LabelProg = null;

    public GameObject m_goProg = null;
    public GameObject m_goBtnTitle = null;
    public GameObject m_goSaveFiles = null;
    
	private IEnumerator LoadLevel(int iSceneIndex)
	{
        m_LabelProg.gameObject.SetActive(true);

        m_Asyn = SceneManager.LoadSceneAsync (iSceneIndex);
        yield return m_Asyn;
	}

	void Update()
	{
		if (m_Asyn != null) 
		{
			m_SpriteProg.fillAmount = m_Asyn.progress;
			string ss = ((int)(m_Asyn.progress * 100)).ToString () + "%";
			m_LabelProg.text = ss;
        }
	}

	int GetSceneIndexBySaveFile(string  stSaveFileNum)
	{
		return 1;
	}

    public void OnTitleClick()
    {
        m_goBtnTitle.SetActive(false);

        UtilFunc.FadeInOut(true);
    }

    public void LoadSaveFile(GameObject go)
    {
        m_goProg.SetActive(true);
        m_goSaveFiles.SetActive(false);

        // 나중에 세이브 파일에서 씬 인덱스도 얻어와야 된다.
        int iSceneIndex = GetSceneIndexBySaveFile(go.name);
        StartCoroutine(LoadSaveFile(iSceneIndex));
    }

    public IEnumerator LoadSaveFile(int iSceneIndex)
    {     
        StartCoroutine(LoadLevel(iSceneIndex));

        yield return new WaitForEndOfFrame();

        // 일단 타이틀에서는 테스트로 기본 마을 사운드를 부르자.
        // 나중에는 테이블에서 읽어오거나 해서 만들자.
        SoundManager.Instance().PlayBGM(SoundManager.eBGMType.eBGM_BaseVill);
        UtilFunc.FadeInOut(true);
    }
}
