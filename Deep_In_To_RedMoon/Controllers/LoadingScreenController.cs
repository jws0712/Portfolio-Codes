namespace OTO.Manager
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEnine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    //Project
    using OTO.Manager;

    public class LoadingScreenController : MonoBehaviour
    {
        static string nextScene = null;

        [SerializeField] private Image progreesBar = null;

        
        private void Start()
        {
            AudioManager.Instance.StopMusic();

            StartCoroutine(Co_LoadSceneProgrees());

            Time.timeScale = 1f;
        }

        
        //씬을 불러옴
        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScreen");
        }

        //로딩 퍼센트를 시각적으로 보여줌
        private IEnumerator Co_LoadSceneProgrees()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            float timer = 0;
            while (!op.isDone)
            {
                yield return null;

                if (op.progress < 0.1f)
                {
                    progreesBar.fillAmount = op.progress;
                }
                else
                {
                    timer += Time.deltaTime;
                    progreesBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);
                    if (progreesBar.fillAmount >= 1f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}

