using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI txtMusic;

    [SerializeField] private List<AudioClip> musicClipList;
    [SerializeField] private AudioSource audioSource;
    private int currentMusicIndex;

    private void Start()
    {
        musicSlider.value = .5f;
        SetVolume();
        PlayRandomMusic();

    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }


    private void PlayRandomMusic()
    {
        currentMusicIndex = Random.Range(0, musicClipList.Count);
        audioSource.clip = musicClipList[currentMusicIndex];
        audioSource.Play();
    }

    public void SetVolume()
    {
        audioSource.volume = musicSlider.value;
        txtMusic.text = "Musica: " + (musicSlider.value * 100).ToString("F0") + "%";
    }

}
