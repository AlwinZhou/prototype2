using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DIalogManager : MonoBehaviour
{
    public string[] texts;
    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    private int currentIndex = 0;
    public AudioClip[] audioClips = new AudioClip[4];
    public AudioSource audio;
    public Sprite[] images = new Sprite[4];
    public Image imageDisplay;
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
            imageDisplay.sprite = images[0];
            AudioSource.PlayClipAtPoint(audioClips[0], transform.position);
        }



        // images[0] = Resources.Load<Sprite>("Assets/pics/pigGun.png");
        //   images[1] = Resources.Load<Sprite>("Assets/pics/lionGun.png");
        // images[2] = Resources.Load<Sprite>("Assets/pics/pigGun.png");
        //images[3] = Resources.Load<Sprite>("Assets/pics/lionGun.png");


    }
    public void nextdialog()
    {
        if (currentIndex < 3)
        {
            textMeshPro.text = texts[currentIndex + 1];
            audio.clip = audioClips[currentIndex + 1];
            audio.Play();
            imageDisplay.sprite = images[currentIndex + 1];
            currentIndex++;
        }
        else
        {
            Debug.Log("enter else");
            SceneManager.LoadScene("Assets/Levels/Level Main/Main Map.unity");
        }
    }
}