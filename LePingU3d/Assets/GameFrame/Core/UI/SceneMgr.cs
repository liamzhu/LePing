using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    public void LoadScene(SceneDefine mSceneDefine, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(mSceneDefine.ToString(), mode);
    }

    public void LoadScene(SceneDefine mSceneDefine, System.Action callback, LoadSceneMode mode = LoadSceneMode.Single)
    {
        CoroutineMgr.Instance.StartCoroutine(LoadSync(mSceneDefine, callback, mode));
    }

    private IEnumerator LoadSync(SceneDefine mSceneDefine, System.Action callback, LoadSceneMode mode = LoadSceneMode.Single)
    {
        yield return SceneManager.LoadSceneAsync(mSceneDefine.ToString(), mode); ;
        if (callback != null)
            callback();
    }

    public AsyncOperation LoadSceneAsync(SceneDefine mSceneDefine, LoadSceneMode mode = LoadSceneMode.Single)
    {
        return SceneManager.LoadSceneAsync(mSceneDefine.ToString());
    }

}

public enum SceneDefine
{
    Logo = 0,
    Login = 1,
    Loading = 2,
    Main = 3
}
