using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeControl : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI txtSFX;
    [SerializeField] private float volume;

    private void Start()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        volume = sfxSlider.value;
        txtSFX.text = "SFX: " + (volume * 100).ToString("F0") + "%";
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volumeSFX", volume);
        GameEvents.GetInstance().SFXChangedVolume();
    }
}
