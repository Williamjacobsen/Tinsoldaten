using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyGateMenu : MonoBehaviour
{
    private TextMeshPro _text;
    [SerializeField] private GameObject PressEField;

    private void Start() 
    {
        if (transform.parent.rotation.z != 0)
        {
            transform.parent.transform.parent.transform.Rotate(new Vector3(0, 0, -90));
        }

        _text = GetComponent<TextMeshPro>();
        UpdateText();
        PressEField.SetActive(false);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Win!");
            SceneManager.LoadScene("SlutCutScene");
        }
    }

    private void FixedUpdate()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"{PlayerStats.keys}/3";
        if (PlayerStats.keys == 3)
        {
            PressEField.SetActive(true);
        }
    }
}
