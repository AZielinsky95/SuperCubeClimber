using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class CustomSceneManager : MonoBehaviour 
{
	private static CustomSceneManager instance;
	public static CustomSceneManager Instance { get { return instance; } }

	void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(gameObject);
		}

		instance = this;

		DontDestroyOnLoad(gameObject);
	}

    public int ActiveSceneIndex
    {
        get { return SceneManager.GetActiveScene().buildIndex; }
    }

    public string NextSceneName(int currentIndex)
    {
        return SceneManager.GetSceneAt(currentIndex+1).name;
    }

    public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

    IEnumerator StartLoadingNextScene(int index)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        yield return async;
        Debug.Log("Loading complete");
    }

    public void LoadSceneAsync(int index, System.Action onComplete)
    {
        StartCoroutine(StartLoadingNextScene(index));
        onComplete();
    }

    public void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void ReplayLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void LoadLevelSelect()
	{
		SceneManager.LoadScene(1);
	}
}
