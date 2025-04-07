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

        
        // 로딩을 실행하는 코드
        private void Start()
        {
            AudioManager.Instance.StopMusic();

            StartCoroutine(Co_LoadSceneProgrees());

            Time.timeScale = 1f;
        }

        
        // 씬을 불러오는 함수
        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScreen");
        }

        // 로딩씬의 기능을 구현한 코루틴
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

