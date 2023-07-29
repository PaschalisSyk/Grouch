using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Image Image;

    private void Start()
    {
        Invoke("FadeIn", 1f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void FadeOut()
    {
        Image.DOFade(1, 2).OnComplete(StartGame);
    }

    void FadeIn()
    {
        Image.DOFade(0, 2f);
    }
}
