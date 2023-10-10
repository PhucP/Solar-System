using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Loading
{
    public class LoadingProcess : MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        [SerializeField] private GameObject startText;
        [SerializeField] private GameObject loadingProcess;
        [SerializeField] private Button solarButton;
        [SerializeField] private Fade fade;
        private float _target;

        private void Start()
        {
            fade.FadeOut( () => fade.gameObject.SetActive(false));
            loadingProcess.SetActive(false);
            startText.gameObject.SetActive(true);
            solarButton.gameObject.SetActive(true);
            startText.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            if (loadingProcess.activeSelf)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, _target, Time.deltaTime);
            }

            if (solarButton.gameObject.activeSelf)
            {
                solarButton.transform.Rotate(Vector3.forward, 30f * Time.deltaTime);
            }
        }

        public async void LoadingScene(string sceneName)
        {
            loadingProcess.SetActive(true);
            startText.gameObject.SetActive(false);
            solarButton.gameObject.SetActive(false);
            
            loadingBar.fillAmount = 0f;
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            
            //run process bar
            do
            {
                await Task.Delay(200);
                _target = scene.progress + 0.1f;
            } while (scene.progress < 0.9f);

            await Task.Delay(1000);
            
            scene.allowSceneActivation = true;
        }
    }
}
