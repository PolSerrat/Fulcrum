using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionsMenu : MonoBehaviour
{
    public GameObject menuPanel; // drag OptionsMenu panel here
    void Start()
    {
        menuPanel.SetActive(true); // visible at scene start
    }
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit pressed");
    }
    public void LoadNormal()
    {
        SceneManager.LoadScene("Normal");
    }
    public void LoadHard()
    {
        SceneManager.LoadScene("Level_Hard");
    }
}