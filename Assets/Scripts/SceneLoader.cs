using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image logo;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AsynchLoad("main"));
    }

    IEnumerator AsynchLoad(string scene)
    {
        yield return null;
        Debug.Log("Starting Load...");

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            //1.0f is reserved for the completion of the activation progress.
            //Values from 0.9 to 1 (extremes excluded) are never used.
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            Debug.Log("Loading Progress:" + (progress * 100) + "%");

            if(asyncOp.progress == 0.9f)
                asyncOp.allowSceneActivation = true;

            yield return null;
        }
    }
}
