using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class AudioManager : ContextProvider<AudioManager>
{
    public bool SoundOn { get; set; } = true;

    private bool _musicOn = true;
    public bool MusicOn
    {
        get
        {
            return _musicOn;
        }

        set
        {
            _musicOn = value;
            if (!_musicOn)
            {
                StopMusic();
            }
        }
    }

    private bool _musicPaused = true;
    public bool MusicPaused
    {
        get
        {
            return _musicPaused;
        }

        set
        {
            PauseMusic(value);
        }
    }

    public void PlaySound(AudioSource source)
    {
        if (SoundOn)
        {
            source.Play();
        }
    }

    public void PlayMusic(AudioSource source)
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _musicSource = null;
        }
        
        if (MusicOn)
        {
            _musicSource = source;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _musicSource = null;
            _musicPaused = false;
        }
    }

    private void PauseMusic(bool pauseMusic)
    {
        if (!MusicOn)
        {
            return;
        }

        if (_musicSource)
        {
            if (!pauseMusic && _musicPaused)
            {
                _musicSource.Play();
                _musicPaused = false;
            }
            else
            {
                _musicSource.Pause();
                _musicPaused = true;
            }
        }
    }

    private AudioSource _musicSource;
}
