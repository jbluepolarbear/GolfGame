using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class GameController : ContextProvider<GameController>
{
    [SerializeField]
    private Animator _rootPanelAnimator;

    [SerializeField]
    private GameObject[] _levelPrefabs;

    [SerializeField]
    private Transform _levelContainer;
    
    [SerializeField]
    private TextMeshProUGUI _levelText;
    
    [SerializeField]
    private TextMeshProUGUI _playLevelText;

    [SerializeField]
    private ToggleButton _soundButton;

    [SerializeField]
    private ToggleButton _musicButton;

    [SerializeField]
    private AudioSource _musicSource;
    
    [SerializeField]
    private StarScoreController _starScoreController;

    [Header("Design Variables")]
    [SerializeField]
    private int _playsUntilVideo = 1;

    [SerializeField]
    private int _levelStartVideoAds = 4;

    public int PlaysUntilVideo => _playsUntilVideo;
    public int StarScore { get; private set; }
    
    public bool Playing { get; private set; }

    public PlayerData PlayerData => _playerData;
    public int NumberOfLevels => _levelPrefabs?.Length ?? 0;

    public int HighestLevel => Math.Min(_playerData.CurrentLevel, NumberOfLevels);

    public bool NextLevelAvailable => _currentLevelPlaying == NumberOfLevels;
    public void RedoLevel()
    {
        CleanUpLevel();
        LoadLevel(_currentLevelPlaying);
    }
    
    public void VideoRedoLevel()
    {
        Analytics.CustomEvent("RestartLevel", new Dictionary<string, object>
        {
            { "CurrentLevel", _currentLevelPlaying }
        });
        CleanUpLevel();
        // ShowVideoLevelEnd(response =>
        // {
        //     LoadLevel(_currentLevelPlaying);
        // });
    }

    public void OpenRestart()
    {
        Context.Get<ModalController>().Open("level_restart");
    }

    public void NextLevel()
    {
        CleanUpLevel();
        _currentLevelPlaying += 1;
        LoadLevel(_currentLevelPlaying);
    }

    public void PlayLevel()
    {
        _rootPanelAnimator.SetBool("play", true);
        LoadLevel(HighestLevel);
    }

    public void PlaySpecificLevel(int level)
    {
        _rootPanelAnimator.SetBool("play", true);
        LoadLevel(level);
    }

    public void GoHome()
    {
        SetPlayLevelText();
        Playing = false;
        _rootPanelAnimator.SetBool("play", false);
        CleanUpLevel();
    }

    public void VideoGoHome()
    {
        Analytics.CustomEvent("QuitLevel", new Dictionary<string, object>
        {
            { "CurrentLevel", _currentLevelPlaying }
        });
        SetPlayLevelText();
        Playing = false;
        
        // ShowVideoLevelEnd(response =>
        // {
        //     _rootPanelAnimator.SetBool("play", false);
        //     CleanUpLevel();
        // });
    }

    public void OpenQuit()
    {
        Context.Get<ModalController>().Open("level_quit");
    }

    public void StarSettled()
    {
        GolfBall golfBall = Context.Get<GolfBall>();
        if (golfBall.TurnsTaken >= _level.MaxTurns)
        {
            LevelFailed();
        }
        else
        {
            StarScore = 1;
            if (golfBall.TurnsTaken + 1 < _level.Par)
            {
                StarScore = 3;
            }
            else if (golfBall.TurnsTaken + 1 == _level.Par)
            {
                StarScore = 2;
            }
        }
        
        _starScoreController.SetStarScore(StarScore);
    }

    public void OpenSettings()
    {
        if (Playing)
        {
            Context.Get<ModalController>().Open("level_settings");
        }
        else
        {
            Context.Get<ModalController>().Open("settings");
            _soundButton.SetState(_playerData.SoundOn);
            _musicButton.SetState(_playerData.MusicOn);
        }
    }

    public void OpenLevelSelect()
    {
        Context.Get<ModalController>().Open("level_select");
    }

    public void ToggleSound()
    {
        _playerData.SoundOn = !_playerData.SoundOn;
        _playerData.Save();
        Context.Get<AudioManager>().SoundOn = _playerData.SoundOn;
        _soundButton.SetState(_playerData.SoundOn);
    }

    public void ToggleMusic()
    {
        _playerData.MusicOn = !_playerData.MusicOn;
        _playerData.Save();
        Context.Get<AudioManager>().MusicOn = _playerData.MusicOn;
        _musicButton.SetState(_playerData.MusicOn);
        Context.Get<AudioManager>().PlayMusic(_musicSource);
    }

    public void OnSupport()
    {
        string subject = UnityWebRequest.EscapeURL("Support: Star Jump");
        string body = UnityWebRequest.EscapeURL(_playerData.Save());
        Application.OpenURL($"mailto:support@birbstudios.com?subject={subject}&body={body}");
    }

    public void OnBirbStudios()
    {
        Application.OpenURL("https://birbstudios.com");
    }

    private void LoadLevel(int level)
    {
        Analytics.CustomEvent("PlayLevel", new Dictionary<string, object>
        {
            { "CurrentLevel", level }
        });
        _levelText.text = $"Level {level}";
        _currentLevelPlaying = level;
        Playing = true;
        StarScore = 3;
        int levelIndex = (level - 1) % _levelPrefabs.Length;
        GetLevelInstance(levelIndex);
        Context.Get<GolfBall>().LevelReset(_level.StartPoint.position, _level.LevelScale, _level.MaxThrust);
        _starScoreController.SetStarScore(3);
    }
    
    private GameObject _levelObject;
    private GameObject GetLevelInstance(int index)
    {
        GameObject levelObject = null;
        if (_levelObject != null)
        {
            levelObject = Instantiate(_levelObject, _levelContainer);
        }
        else
        {
            levelObject = Instantiate(_levelPrefabs[index], _levelContainer);
        }
        return levelObject;
    }

    public void LevelSuccess()
    {
        Analytics.CustomEvent("LevelSuccess", new Dictionary<string, object>
        {
            { "CurrentLevel", _playerData.CurrentLevel },
            { "StarScore", StarScore }
        });
        
        if (_currentLevelPlaying > _playerData.LevelInfos.Length)
        {
            _playerData.CurrentLevel = _currentLevelPlaying + 1;
            _playerData.LevelInfos = _playerData.LevelInfos.Append(new PlayerData.LevelInfo
            {
                StarCount = StarScore
            }).ToArray();
            _playerData.Save();
        } 
        else if (StarScore > _playerData.LevelInfos[_currentLevelPlaying - 1].StarCount)
        {
            _playerData.LevelInfos[_currentLevelPlaying - 1].StarCount = StarScore;
            _playerData.Save();
        }
        
        Context.Get<GolfBall>().SetActing(false);
        // ShowVideoLevelEnd(response =>
        // {
        //     if (_currentLevelPlaying != NumberOfLevels)
        //     {
        //         _modalController.Open("level_finished");
        //     }
        //     else
        //     {
        //         _modalController.Open("level_failed");
        //     }
        // });
    }

    private void LevelFailed()
    {
        Analytics.CustomEvent("LevelFailed", new Dictionary<string, object>
        {
            { "CurrentLevel", _playerData.CurrentLevel },
            { "StarScore", StarScore }
        });
        
        StarScore = 0;
        Context.Get<GolfBall>().SetActing(false);
        // ShowVideoLevelEnd(response =>
        // {
        //     _modalController.Open("level_failed");
        // });
    }

    private void CleanUpLevel()
    {
        Context.Get<GolfBall>().SetActing(false);
        if (_level)
        {
            _level.Close();
        }
        _level = null;
    }
    
    private void OnEnable()
    {
        _playerData = PlayerData.Load();
        if (Context.Has<AudioManager>())
        {
            Context.Get<AudioManager>().SoundOn = _playerData.SoundOn;
            Context.Get<AudioManager>().MusicOn = _playerData.MusicOn;
            if (_musicSource)
            {
                Context.Get<AudioManager>().PlayMusic(_musicSource);
            }
        }
        SetPlayLevelText();
    }

    private void OnDisable()
    {
        _playerData?.Save();
        _playerData = null;
    }

    private IEnumerator Start()
    {
        Application.targetFrameRate = 60;
        GameObject levelObject = GetComponentInChildren<Level>()?.gameObject;
        if (levelObject != null && levelObject.activeSelf)
        {
            _levelObject = levelObject;
            PlayLevel();
        }
        else if (HighestLevel == 1)
        {
            PlayLevel();
        }
        
        yield break;
        
        // while (!Context.Get<AdProvider>().BannerReady)
        // {
        //     yield return null;
        // }
        // Context.Get<AdProvider>().ShowBanner();
    }

    private void SetPlayLevelText()
    {
        if (_playLevelText != null)
        {
            _playLevelText.text = $"Level {HighestLevel}";
        }
    }

    // private void ShowVideoLevelEnd(AdProvider.OnVideoFinishedCallback callback)
    // {
    //     if (HighestLevel < _levelStartVideoAds)
    //     {
    //         Debug.Log($"Video Callback: Current Level {HighestLevel} is less than requirement: {_levelStartVideoAds}");
    //         callback?.Invoke(AdProvider.VideoResponse.Success);
    //         return;
    //     }
    //     
    //     _playerData.PlaysUntilVideo -= 1;
    //     if (_playerData.PlaysUntilVideo > 0)
    //     {
    //         Debug.Log($"Video Callback: {_playerData.PlaysUntilVideo} more plays until video.");
    //         callback?.Invoke(AdProvider.VideoResponse.Success);
    //         return;
    //     }
    //     
    //     _playerData.PlaysUntilVideo = _playsUntilVideo;
    //     _playerData.Save();
    //     
    //     Context.Get<AdProvider>().ShowVideo(response =>
    //     {
    //         Debug.Log($"Video Callback: Response: {response}.");
    //         callback?.Invoke(response);
    //     });
    // }

    private Level _level;
    private PlayerData _playerData;
    private int _currentLevelPlaying;
}
