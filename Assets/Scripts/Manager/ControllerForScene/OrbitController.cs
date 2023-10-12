using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    private void Start()
    {
        fade.FadeOut( () => fade.gameObject.SetActive(false));
    }
    
    public void BackToHome()
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
            SceneManager.LoadScene(Constant.HOME_SCENE_NAME); 
        });
    }

    public void ShowHideOptionsPanel()
    {
        
    }

    public void ChandeViewCamera()
    {
        
    }

    public void ChangeSpeedCelestial()
    {
        
    }
    
    
}
