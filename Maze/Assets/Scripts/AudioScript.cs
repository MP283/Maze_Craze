using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    // Static instance of the GameManager which allows it to be accessed by any other script.
    public static AudioScript Instance { get; private set; }

	public AudioMixer AudioMix;
    public Slider BackgroundMusicVolumeSlider, GameSoundsSlider;


    private void Start(){
        Time.timeScale = 1f;
        float currentBackgroundMusicVolume, currentGameSoundsVolume;

        AudioMix.GetFloat("Volume", out currentBackgroundMusicVolume);
        AudioMix.GetFloat("VolumeForGameSounds", out currentGameSoundsVolume);

        BackgroundMusicVolumeSlider.value = currentBackgroundMusicVolume;
        GameSoundsSlider.value = currentGameSoundsVolume;

        SetVolume(currentBackgroundMusicVolume);
        SetVolumeForGameSounds(currentGameSoundsVolume);

        // Add a listener to the slider to call the SetVolume method
        BackgroundMusicVolumeSlider.onValueChanged.AddListener(SetVolume);
        GameSoundsSlider.onValueChanged.AddListener(SetVolumeForGameSounds);
    }

    // Example of a method that might be in a GameManager.
    public void SetVolume(float volume){
		AudioMix.SetFloat("Volume", volume);
	}

    public void SetVolumeForGameSounds(float volume){
		AudioMix.SetFloat("VolumeForGameSounds", volume);
	}

    public void SetQuality(int quality){

		QualitySettings.SetQualityLevel(quality);

	}
}


    /*
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Check if an instance of GameManager already exists
        if (Instance == null)
        {
            // If not, set it to this.
            Instance = this;
            // Make this instance persistent across scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this to enforce the singleton.
            Destroy(gameObject);
        }
    }*/