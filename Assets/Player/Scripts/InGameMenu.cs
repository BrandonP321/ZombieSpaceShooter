using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    public GameObject menuUI;
    public bool isVisible = false;

    public void Start()
    {
        menuUI.SetActive(isVisible);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMenu()
    {
        isVisible = !isVisible;
        menuUI.SetActive(isVisible);
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isVisible;
    }
}
