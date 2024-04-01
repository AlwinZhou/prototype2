using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayMap1()
    {
        SceneManager.LoadScene("Assets/RedBjorn/SuperTiles/Examples/Square 2/Square 2.unity");
    }
}
