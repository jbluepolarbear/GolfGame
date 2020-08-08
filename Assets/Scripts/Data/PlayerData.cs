﻿using System;
using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Analytics;

 namespace Data
{
    [Serializable]
    public class PlayerData
    {
        [Serializable]
        public class LevelInfo
        {
            public int StarCount = 1;
        }
        public int CurrentLevel = 1;
        public LevelInfo[] LevelInfos = {};
        public int PlaysUntilVideo = 0;
        public bool MusicOn = true;
        public bool SoundOn = true;

        public string Save()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString("player_data", json);
            return json;
        }

        public static PlayerData Load()
        {
            if (PlayerPrefs.HasKey("player_data"))
            {
                return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("player_data"));
            }
            
            Analytics.CustomEvent("NewPlayer");

            return new PlayerData();
        }
    }
}