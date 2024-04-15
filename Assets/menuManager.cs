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

    public void playWorldMap()
    {
        SceneManager.LoadScene("Assets/Scenes/WorldMap.unity");
    }

    public void OpenMusicManager()
    {
        MusicPanel.SetActive(true);
        settingButton.SetActive(false);
    }

    public void CloseMusicManager()
    {
        MusicPanel.SetActive(false);
        settingButton.SetActive(true);

    }
}
