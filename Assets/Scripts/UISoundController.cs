using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class UISoundController : MonoBehaviour
{
    public UISoundController()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public static UISoundController Instance { get; private set; }
    
    [SerializeField]
    private AudioClip _openMenuClip;
    [SerializeField]
    private AudioClip _closeMenuClip;
    [SerializeField]
    private AudioClip _buttonClickClip;
    [SerializeField]
    private AudioSource _audioSource;

    public void PlayOpenMenu()
    {
        PlayClip(_openMenuClip);
    }
    
    public void PlayCloseMenu()
    {
        PlayClip(_closeMenuClip);
    }
    
    public void PlayButtonClick()
    {
        PlayClip(_buttonClickClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        
        _audioSource.clip = clip;
        Context.Get<AudioManager>().PlaySound(_audioSource);
    }
}
