using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusica : MonoBehaviour
{
    public AudioClip[] PlayList;
    public float fadeSpeed = 2f;

    private AudioSource audioSource;
    private float alpha = 1.0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if(!audioSource.playOnAwake)
            audioSource.clip = PlayList[Random.Range(0, PlayList.Length)] as AudioClip;

        audioSource.Play();
        StartCoroutine(Fade());
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            playRandomMusic();
            StartCoroutine(Fade());
        }        
    }

    void playRandomMusic()
    {
        audioSource.clip = PlayList[Random.Range(0, PlayList.Length)] as AudioClip;
        audioSource.Play();
    }

    IEnumerator Fade()
    {
        alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("FadeOut");
        while(alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    /*private void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        GUI.color = new Color(255, 255, 0, alpha);
        myStyle.fontSize
    }*/
}
