using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Start() {
        Screen.SetResolution(1024, 768, true, 60);
    }

    public void OnStartButton() {
        SceneManager.LoadScene("Game");
    }

    public void OnExitButton() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
