using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GearWheel : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;

    private void Awake() 
    {
        Canvas.SetActive(false);    
    }

    private void OnMouseEnter()
    {
    }

    private void OnMouseExit()
    {
    }

    private void OnMouseDown() 
    {
        PauseGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Canvas.SetActive(true);
            PauseMenu.SetActive(true);

            Time.timeScale = 0;
        }
        else
        {
            PauseMenu.SetActive(true);
            SettingsMenu.SetActive(false);
            Canvas.SetActive(false);

            Time.timeScale = 1;
        }
    }
}
