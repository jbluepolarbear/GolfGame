using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
    [SerializeField]
    private StarScoreController _starScoreController;

    [SerializeField]
    private TextMeshProUGUI _levelText;

    [SerializeField]
    private GameObject _locked;
    
    [SerializeField]
    private Button _button;

    public delegate void LevelSelectedCallback(int level);

    public void SetLevelItem(int score, int level, bool locked, LevelSelectedCallback callback)
    {
        _locked.SetActive(locked);
        _level = level;
        _levelText.text = $"Level {level}";
        _starScoreController.SetStarScore(score, false);
        _callback = callback;
        _button.interactable = !locked;
    }

    public void Select()
    {
        _callback?.Invoke(_level);
    }

    private int _level;
    private LevelSelectedCallback _callback;
}
