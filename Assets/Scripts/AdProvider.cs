// using System;
// using System.Collections;
// using System.Collections.Generic;

using Contexts;

// using UnityEngine;
// using UnityEngine.Advertisements;

// public class AdProvider : ContextProvider<AdProvider>, IUnityAdsListener
// {
//     public AdProvider()
//     {
//         if (!Instance)
//         {
//             Instance = this;
//         }
//     }
//     
//     public enum VideoResponse
//     {
//         Success,
//         Failed,
//         AlreadyShowing,
//         NotInitialized
//     }
//
//     public delegate void OnVideoFinishedCallback(VideoResponse response);
//     
//     public bool BannerReady { get; private set; }
//     public bool VideoReady { get; private set; }
//     
//     public static AdProvider Instance { get; private set; }
//     
//     private void Awake()
//     {
//         Initialize();
//     }
//
//     public void ShowBanner()
//     {
//         if (!BannerReady)
//         {
//             Initialize();
//             return;
//         }
//         
//         Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
//         Advertisement.Banner.Show("LevelBanner");
//     }
//
//     public void HideBanner()
//     {
//         if (!BannerReady)
//         {
//             Initialize();
//             return;
//         }
//         Advertisement.Banner.Hide(false);
//     }
//     
//     public void ShowVideo(OnVideoFinishedCallback callback)
//     {
//         if (!VideoReady)
//         {
//             Initialize();
//             _callback?.Invoke(VideoResponse.NotInitialized);
//             return;
//         }
//
//         if (Advertisement.isShowing)
//         {
//             _callback?.Invoke(VideoResponse.AlreadyShowing);
//             return;
//         }
//         
//         _callback = callback;
//         Context.Get<AudioManager>().MusicPaused = true;
//         Advertisement.Show("LevelComplete");
//     }
//     
//     public void OnUnityAdsReady(string placementId)
//     {
//         if (placementId == "LevelComplete")
//         {
//             VideoReady = true;
//         }
//
//         if (placementId == "LevelBanner")
//         {
//             BannerReady = true;
//         }
//     }
//
//     public void OnUnityAdsDidError(string message)
//     {
//         Debug.LogError("OnUnityAdsDidError: " + message);
//         _callback?.Invoke(VideoResponse.Failed);
//     }
//
//     public void OnUnityAdsDidStart(string placementId)
//     {
//     }
//
//     public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
//     {
//         Context.Get<AudioManager>().MusicPaused = false;
//         _callback?.Invoke(VideoResponse.Success);
//     }
//
//     private void Initialize()
//     {
//         if (Advertisement.isInitialized)
//         {
//             return;
//         }
//         
// #if UNITY_ANDROID
//         string gameId = "3738837";
// #else
//         string gameId = "3738836";
// #endif
//         Advertisement.AddListener(this);
//         Advertisement.Initialize(gameId, Debug.isDebugBuild);
//     }
//
//     private OnVideoFinishedCallback _callback;
// }

public class AdProvider : ContextProvider<AdProvider>
{
    
}