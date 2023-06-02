using UnityEngine;

public class LoadScene : MonoBehaviour
{
	public void Load(string sceneName)
	{
		#if UNITY_EDITOR
		
		#endif

		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
