using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (IsPaused){
                Resume();
            }
            else {
                Pause();
            }
        }
    }
    public void Resume(){
        Time.timeScale = 1f;
        IsPaused = false;
        PauseMenu.SetActive(false);
    }
    public void Pause() {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        IsPaused = true;
    }

    public void ExitToMainMenu() {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
