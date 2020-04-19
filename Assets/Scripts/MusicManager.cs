using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicMain;
    [SerializeField] AudioSource musicQuestion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioFade.FadeIn(musicMain, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwapMusic(AudioSource newMusic)
    {

    }
}

public static class AudioFade 
{
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float targetVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= targetVolume * Time.deltaTime / fadeTime;
            Debug.Log("Music Vol: " + audioSource.volume.ToString());
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = targetVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeTime;

            yield return null;
        }
    }
}
