using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menuManager : MonoBehaviour
{
    public void playWorldMap()
    {
        SceneManager.LoadScene("Assets/Scenes/WorldMap.unity");
    }
}
