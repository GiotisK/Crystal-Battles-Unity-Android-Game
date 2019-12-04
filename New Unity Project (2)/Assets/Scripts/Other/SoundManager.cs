using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
    private Sprite SoundOnSprite;
    private Sprite SoundOffSprite;
    private Sprite CurrentSprite;
    private Image ButtonImage;
    public AudioClip Smash;
    public AudioClip Crack;
    public AudioClip PowerUp;
    public AudioSource asrc1;
    public AudioSource asrc2;
    public bool SoundOn = true;


    void Start () {
        SoundOnSprite = Resources.Load("SoundOn", typeof(Sprite)) as Sprite;
        SoundOffSprite = Resources.Load("SoundOff", typeof(Sprite)) as Sprite;
        ButtonImage = GameObject.Find("SoundButton").GetComponent<Image>();
    }
	

	public void OnButtonClick () {
        CurrentSprite = ButtonImage.sprite;
        if (CurrentSprite == SoundOnSprite)
        {
            SoundOn = false;
            ButtonImage.sprite = SoundOffSprite;
        }
        else if (CurrentSprite == SoundOffSprite)
        {
            SoundOn = true;
            ButtonImage.sprite = SoundOnSprite;
        }
    }

    public void PlayPowerUpSound()
    {
        if (SoundOn)
        {
            asrc1.PlayOneShot(PowerUp);
        }
    }
    public void PlaySmashSound()
    {
        if (SoundOn)
        {
            asrc1.PlayOneShot(Smash);
        }      
    }

    public void PlayCrackSound()
    {
        if (SoundOn)
        {
            asrc2.PlayOneShot(Crack);
        }     
    }
}
