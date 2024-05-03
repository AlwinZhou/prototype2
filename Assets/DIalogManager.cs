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
    public AudioClip[] audioClips = new AudioClip[7];
    public AudioSource audio;
    public Sprite[] images = new Sprite[7];
    public Image imageDisplay;
    // Start is called before the first frame update
    void Start()
    {
        texts = new string[7];
        texts[0] = "DOG: This way, everyone! We're almost at the Hitundra temple!\r\n";
        texts[1] = "HORSE: Chief, for all we know this could be a trap laid out by the Commune. How can you be sure it's safe?\r\n";
        texts[2] = "DOG: I can't, but we're out of options. Besides,the temple used to be a rebel base before the war, so maybe there is something useful here.\r\n";
        texts[3] = "DOG: If nothing else, it will serve as a good holdout position until we can find a way to escape.\r\n";
        texts[4] = "HORSE: Chief! More Commune reinforcements are closing in on us, and fast! There's so many of them...!\r\n";
        texts[5] = "DOG: Did the Commune send their whole army!? Do not let them overwhelm us! Hold out as long as you can!\r\n";
        texts[6] = "DOG: We're in for the fight of our lives...\r\n";
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
        if (currentIndex < 6)
        {
            textMeshPro.text = texts[currentIndex + 1];
           // audio.clip = audioClips[currentIndex + 1];
//audio.Play();
            imageDisplay.sprite = images[currentIndex + 1];
            currentIndex++;
        }
        else
        {
            Debug.Log("enter else");
            SceneManager.LoadScene("Assets/Levels/Level 2/Level 2.unity");
        }
    }
}