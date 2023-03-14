using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXControl : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;


    private void Start()
    {
        GameEvents.GetInstance().OnSFXChangedVolume += SetAudioSourveVolume;
    }

    private void SetAudioSourveVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("volumeSFX");
    }

    public void PlayClip()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return null;

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
