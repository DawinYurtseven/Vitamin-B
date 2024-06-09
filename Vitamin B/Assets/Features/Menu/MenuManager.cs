using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public void ExitButton() {
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("ArtScene");
    }
    public void ChangeVolume(Slider slider) {
        mainAudioMixer.SetFloat("MasterVolume", slider.value);
    }
}
