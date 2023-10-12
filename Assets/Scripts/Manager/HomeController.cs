using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : Singleton<HomeController>
{
    [Header("Fades")]
    [SerializeField] private Fade fade;
    
    protected override void Awake()
    {
        base.Awake();
    }
    

    private void Start()
    {
        fade.FadeOut( () => fade.gameObject.SetActive(false));
    }

    public void LoadScene(string sceneName)
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
    
}
