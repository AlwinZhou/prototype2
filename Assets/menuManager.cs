using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menuManager : MonoBehaviour
{
    //gameObject assigning
    [SerializeField]
    private GameObject MusicPanel;
    [SerializeField]
    private GameObject settingButton;
    [SerializeField]
    private GameObject TutorialPanel;
    [SerializeField]
    private GameObject settingButton2;
    [SerializeField]
    private GameObject tutorialButton;


    public void playWorldMap()
    {
        SceneManager.LoadScene("Assets/Scenes/Dialogues.unity");
    }

    public void OpenMusicManager()
    {
        MusicPanel.SetActive(true);
        settingButton.SetActive(false);
        tutorialButton.SetActive(false);
    }

    public void CloseMusicManager()
    {
        MusicPanel.SetActive(false);
        settingButton.SetActive(true);
        tutorialButton.SetActive(true);


    }

    public void OpenTutorial()
    {
        TutorialPanel.SetActive(true);
        settingButton2.SetActive(true);
        tutorialButton.SetActive(false);

    }

    public void CloseTutorial()
    {
        TutorialPanel.SetActive(false);
        settingButton2.SetActive(false);
        tutorialButton.SetActive(true);


    }
}
