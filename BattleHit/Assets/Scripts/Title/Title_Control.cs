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

	private AsyncOperation asyn = null;

	public UISprite prog = null;
	public UILabel progLabel = null;

	// Use this for initialization
	void Start () 
	{
		int iSceneIndex = GetSceneIndexBySaveFile ();
		StartCoroutine (LoadLevel(iSceneIndex));
	}
	
	private IEnumerator LoadLevel(int iSceneIndex)
	{
		asyn = SceneManager.LoadSceneAsync (iSceneIndex);
		yield return asyn;
	}

	void Update()
	{
		if (asyn != null) 
		{
			prog.fillAmount = asyn.progress;
			string ss = (asyn.progress * 100).ToString () + "%";
			progLabel.text = ss;
		}
	}

	int GetSceneIndexBySaveFile()
	{
		return 1;
	}
}
