using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicMain;
    [SerializeField] AudioSource musicQuestion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioFade.FadeIn(musicMain, Time.time, 5.0f));
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
    public static IEnumerator FadeOut(AudioSource audioSource, float startTime, float fadeTime)
    {
        while (audioSource.volume > 0)
        {
            //audioSource.volume -= targetVolume * Time.deltaTime / fadeTime;
            //Debug.Log("Music Vol: " + audioSource.volume.ToString());
            float deltaTime = (Time.time - startTime) / fadeTime;
            audioSource.volume = Mathf.SmoothStep(1.0f, 0.0f, deltaTime);

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float startTime, float fadeTime)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            //audioSource.volume += targetVolume * Time.deltaTime / fadeTime;
            float deltaTime = (Time.time - startTime) / fadeTime;
            audioSource.volume = Mathf.SmoothStep(0.0f, 1.0f, deltaTime);

            yield return null;
        }
    }
}
