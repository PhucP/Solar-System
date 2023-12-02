using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour
{
    [Header("Fades")]
    [SerializeField] protected Fade fade;

    protected virtual void Start()
    {
        fade.FadeOut( () => fade.gameObject.SetActive(false));
    }

    public virtual void LoadScene(string sceneName)
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneName); 
        });
    }

    public virtual void BackToHome()
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
            //reset time scale
            Time.timeScale = 1;
            SceneManager.LoadScene(Constant.HOME_SCENE_NAME); 
        });
    }

    public virtual void OpenOption()
    {
        MainController.Instance.ShowHideOptionPopup(true);
    }
}
