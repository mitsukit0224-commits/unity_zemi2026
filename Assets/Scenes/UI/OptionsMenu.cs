using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider volumeSlider;

    private bool isOpen = false;

    void Start()
    {
        optionsPanel.SetActive(false);
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void ToggleOptions()
    {
        isOpen = !isOpen;
        optionsPanel.SetActive(isOpen);
    }

    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}