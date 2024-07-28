using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayerScript : MonoBehaviour
{
    
    public GameObject objectMusic;
    public Slider volumeSlider;
    private float musicVolume = 1f;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        objectMusic = GameObject.FindWithTag("GameMusic");
        audioSource = objectMusic.GetComponent<AudioSource>();

        musicVolume = PlayerPrefs.GetFloat("volume");
        audioSource.volume = musicVolume;
        volumeSlider.value = musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("volume", musicVolume);
    }

    public void updateVolume(float volume)
    {
        musicVolume = volume;
    }
}
