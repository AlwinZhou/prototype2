using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Controller of all audio-related features
    /// </summary>
    public class AudioController
    {

        static AudioSource GetSource()
        {
            var go = new GameObject("AudioSource");
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            return go.AddComponent<AudioSource>();
        }

        public static void PlayButtonClick()
        {
            PlaySound(S.Sound.ButtonUiClick);
        }

        public static void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                var source = GetSource();
                source.clip = clip;
                source.Play();
                GameObject.Destroy(source.gameObject, source.clip.length);
            }
        }

    }
}