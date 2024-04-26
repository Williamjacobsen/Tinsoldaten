using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneText : MonoBehaviour
{
    [SerializeField] private GameObject FadeIn;
    [SerializeField] private GameObject FadeOut;

    [SerializeField] private GameObject Canvas;

    [SerializeField] private AudioSource Source;

    [SerializeField] private SpriteRenderer CutSceneSpriteRenderer;
    [SerializeField] private Sprite NextCutSceneSprite;

    [SerializeField] private GameObject LastCutScene;

    private Dictionary<string, float> TextAndTime;

    private TextMeshPro Text;

    private void Awake()
    {
        Canvas.SetActive(false);
        Text = GetComponent<TextMeshPro>();

        if (SceneManager.GetActiveScene().name == "SlutCutScene")
        {
            TextAndTime = new()
            {
                {"Porten førte op til selvsamme stue med det dejlige slot hvori den nydelige lille danserinde boede", 6},
                {"Den ene af smådrengene fik øje på den lille tinsoldat og kastede ham lige ind i kakkelovnen", 5.5f},
                {"Og samtidig tog vinden i danserinden og hun fløj ligesom en sylfide hen til tinsoldaten, blussede op i lue og var borte", 7.5f},
                {"så smeltede tinsoldaten til en klat, et lille tinhjerte", 4.3f},
                {"af danserinden derimod var der kun pailletten, og den var brændt kulsort", 4.7f},
            };
        }
        else
        {
            TextAndTime = new()
            {    
                {"Der var engang femogtyve tinsoldater, de var alle brødre, thi de var født af en gammel tin-ske.", 5},
                {"Det var alt sammen nydeligt, men det nydeligste blev dog en lille jomfru af papir", 5},
                {"Den lille jomfru strakte begge sine arme ud, for hun var en danserinde", 4},
                {"og så løftede hun sit ene ben så højt i vejret, at tinsoldaten slet ikke kunne finde det og troede, at hun kun havde ét ben ligesom han", 7},
                {"'Det var en kone for mig!' tænkte han", 12f},
                {"Nu slog klokken tolv, og klask, der sprang låget af snustobaksdåsen, men der var ingen tobak i", 6},
                {"nej, men en lille sort trold, det var sådant et kunststykke", 5},
                {"'Tinsoldat!' sagde trolden, 'vil du holde dine øjne hos dig selv!'", 3.5f},
                {"Men tinsoldaten lod, som han ikke hørte det.", 2.4f},
                {"'Ja bare vent til i morgen!' sagde trolden", 5f},
                {"Da det nu blev morgen, og børnene kom op, blev tinsoldaten stillet hen i vinduet, og enten det nu var trolden eller trækvind", 5.5f},
                {"lige med ét fløj vinduet op og soldaten gik ud på hovedet fra tredje sal", 5},
                {"Change Scene", 0},
                {"Nu begyndte det at regne, den ene dråbe faldt tættere end den anden, det blev en ordentlig skylle; da den var forbi, kom der to gadedrenge", 7.5f},
                {"'Se du!' sagde den ene, 'der ligger en tinsoldat! Han skal ud at sejle!'", 4.5f},
                {"Og så gjorde de en båd af en avis, satte tinsoldaten midt i den, og nu sejlede han ned af rendestenen mod det ukendte", 8f},
            };
        }

        StartCoroutine(ShowParagrafAfterTime());

        Settings.HaveCutScenePlayed = true;
    }

    private void Update() 
    {
        if (!Canvas.activeSelf && Time.timeScale == 0)
        {
            Source.UnPause();
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (Time.timeScale == 0)
            {
                Canvas.SetActive(false);
                Source.UnPause();
                Time.timeScale = 1;
            }
            else
            {
                Source.Pause();
                Time.timeScale = 0;
                Canvas.SetActive(true);
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        Text.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(0.025f);
            Text.text += text[i];
        }
    }

    private IEnumerator ShowParagrafAfterTime()
    {
        yield return new WaitForSeconds(1);
        Source.Play();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < TextAndTime.Count; i++)
        {
            if (TextAndTime.ElementAt(i).Key == "Change Scene")
            {
                Source.Pause();
                yield return new WaitForSeconds(2);
                FadeIn.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                CutSceneSpriteRenderer.sprite = NextCutSceneSprite;
                FadeOut.SetActive(false);
                FadeOut.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                FadeIn.SetActive(false);
                yield return new WaitForSeconds(1);
                Source.UnPause();
                continue;
            }

            StartCoroutine(TypeText(TextAndTime.ElementAt(i).Key));
            yield return new WaitForSeconds(TextAndTime.ElementAt(i).Value);
        }

        if (SceneManager.GetActiveScene().name != "SlutCutScene")
        {
            LastCutScene.SetActive(true);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
