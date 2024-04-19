using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class DIalogManager : MonoBehaviour
{
    public string[] texts;
    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;
    public  AudioClip[] audioClips = new AudioClip[4];

    // Start is called before the first frame update
    void Start()
    {
        texts = new string[4];
        texts[0] = "Fighter Pig: Protect all animals! Together, we're stronger. Let's defend our habitats and show unity. We fight for justice and our animal kingdom!";
        texts[1] = "Fighter Lion: Preserve nature's balance! With courage and claws, we fight. Roar with determination, for all beings depend on us. Together, we prevail in this noble battle!";
        texts[2] = "Fighter Pig: Every life matters. Let's shield our animal friends. Together, we're an unstoppable force!";
        texts[3] = "Fighter Lion: Nature's guardians, united we stand. With bravery, we defend our fellow creatures. Our roar echoes with purpose. Together, we emerge victorious!";
        if (texts.Length > 0)
        {
            textMeshPro.text = texts[0];
        }

        

       
    }

    public void nextdialog()
    {
        if (currentIndex < 4)
        {
            textMeshPro.text = texts[currentIndex + 1];
            Debug.Log(","+ texts.Length);
            currentIndex++;
        }
        if (currentIndex == 4)
        {
            Debug.Log("enter else");
            SceneManager.LoadScene(11);
        }
    }
}
