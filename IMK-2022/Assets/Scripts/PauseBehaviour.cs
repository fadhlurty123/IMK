using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehaviour : MonoBehaviour
{

    public GameObject GameUI;
    public GameObject PauseUI;

    void Start()
    {
        PauseUI = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        GameUI.SetActive(false);
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameUI.SetActive(true);
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }
}