using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject _MainMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject BrightMode;

    private void Awake()
    {
        try
        {
            if (name == "BackBtn")
            {
                return;
            }
            SettingsMenu.SetActive(false);
        }
        catch {} // not all MainMenu.cs Scripts has the gameobjects
    }

    private void Start()
    {
        try
        {
            if (Settings.LightMode == 2)
            {
                BrightMode.SetActive(true);
            }
            else
            {
                BrightMode.SetActive(false);
            }
        }
        catch {} // not all MainMenu.cs Scripts has the gameobjects
    }

    private void OnEnable()
    {
        if (name == "Canvas" && SceneManager.GetActiveScene().name == "Main")
        {
            SettingsMenu.SetActive(false);
            _MainMenu.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (name == "Canvas" && SceneManager.GetActiveScene().name == "Main" && BrightMode != null)
        {
            if (Settings.LightMode == 2)
            {
                BrightMode.SetActive(true);
            }
            else
            {
                BrightMode.SetActive(false);
            }
        }
    }

    public void Larger()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);       
        transform.position = new Vector2(transform.position.x - 10, transform.position.y); 
    }

    public void Smaller()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector2(transform.position.x + 10, transform.position.y);
    }

    public void StartGame()
    {
        //Debug.Log(Settings.LightMode);

        Smaller();

        if (!Settings.HaveCutScenePlayed)
        {
            PlayCutScene();
            return;
        }

        //if (SceneManager.GetActiveScene().name == "Main")
        //{
        //    _MainMenu.SetActive(false);
        //    SettingsMenu.SetActive(false);
        //}
        
        // Reset
        PlayerStats.health = 1;
        PlayerStats.keys = 0;
        Time.timeScale = 1;

        SceneManager.LoadScene("Main");

        //Debug.Log(Settings.LightMode);
    }

    private void PlayCutScene()
    {
        SceneManager.LoadScene("StartCutScene");
    }

    public void SettingsBtn()
    {
        Smaller();

        //SceneManager.LoadScene("Settings");
        SettingsMenu.SetActive(true);
        _MainMenu.SetActive(false);
    }

    public void Exit()
    {        
        Application.Quit();
        Debug.Log("Game Closed");
    }

    [SerializeField] private string BackSceneName;
    public void Back()
    {
        Smaller();
        
        //SceneManager.LoadScene(BackSceneName);
        //SceneManager.LoadScene(Settings.PreviousScene);
        SettingsMenu.SetActive(false);
        _MainMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Smaller();
        
        SceneManager.LoadScene("Main Menu");
    }

    public void Continue()
    {
        //Debug.Log(Settings.LightMode);

        //GameObject.Find("Canvas").SetActive(false);
        Smaller();

        _MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Canvas.SetActive(false);  

        Time.timeScale = 1;    

        //Debug.Log(Settings.LightMode);
    }

    public void SkipCutScene()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "SlutCutScene")
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void ResumeCutScene()
    {
        Canvas.SetActive(false);
    }

    public void LightNormalMode()
    {
        Settings.LightMode = 1;
    }

    public void LightBrightMode()
    {
        Settings.LightMode = 2;
    }

    public void LightIntensity()
    {
        Settings.LightIntensity = GetComponent<Slider>().value * 4;
        Debug.Log(Settings.LightIntensity);
    }

    public void ResetLightSettings()
    {
        Settings.LightMode = 1;
        GameObject.Find("Normal").GetComponent<Toggle>().isOn = true;
        Settings.LightIntensity = 1;
        GameObject.Find("Slider").GetComponent<Slider>().value = 0.25f;
    }
}
