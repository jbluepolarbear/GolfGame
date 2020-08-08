using System;
using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject _levelSelectGroupPrefab;

    [SerializeField]
    private Transform _groupContainer;
    
    [SerializeField]
    public ScrollRect _scrollRect;

    [SerializeField]
    public Scrollbar _Scrollbar;

    private void Start()
    {
        Refresh();
        _scrollRect.StopMovement();
    }

    private void OnEnable()
    {
        _scrollRect.StopMovement();
        _scrollRect.enabled = false;
        _groupContainer.localPosition = Vector3.zero;
        Refresh();
        StartCoroutine(WaitForEnable());
    }

    private IEnumerator WaitForEnable()
    {
        yield return new WaitForSeconds(0.5f);
        _scrollRect.StopMovement();
        _scrollRect.enabled = true;
        _groupContainer.localPosition = Vector3.zero;
    }
    
    private void OnDisable()
    {
        _scrollRect.StopMovement();
        _Scrollbar.value = 0;
        _scrollRect.enabled = false;
        _groupContainer.localPosition = Vector3.zero;
    }

    private void Refresh()
    {
        const int itemsPerGroup = 3;
        int numGroups = Context.Get<GameController>().NumberOfLevels / itemsPerGroup;
        if (Context.Get<GameController>().NumberOfLevels % itemsPerGroup > 0)
        {
            numGroups += 1;
        }
        
        for (int i = 0, j = 0; i < numGroups; ++i)
        {
            LevelSelectGroup group;
            if (_listSelectGroups.Count > i)
            {
                group = _listSelectGroups[i];
            }
            else
            {
                GameObject groupGameObject = Instantiate(_levelSelectGroupPrefab, _groupContainer);
                group = groupGameObject.GetComponent<LevelSelectGroup>();
                _listSelectGroups.Add(group);
            }

            for (int k = 0; k < itemsPerGroup; ++k, ++j)
            {
                if (j >= Context.Get<GameController>().NumberOfLevels)
                {
                    group.DisableSelectItem(k);
                    continue;
                }
                bool locked = false;
                int score = 0;
                if (j < Context.Get<GameController>().PlayerData.LevelInfos.Length)
                {
                    score = Context.Get<GameController>().PlayerData.LevelInfos[j].StarCount;
                }
                else if (j + 1 != Context.Get<GameController>().PlayerData.CurrentLevel)
                {
                    locked = true;
                }
                group.SetLevelSelectItem(
                    k,
                    score,
                    j + 1,
                    locked,
                    SelectedCallback);
            }
        }
    }

    private ModalBase _modalBase;
    private void SelectedCallback(int level)
    {
        Context.Get<GameController>().PlaySpecificLevel(level);
        if (_modalBase == null)
        {
            _modalBase = GetComponentInParent<ModalBase>();
        }
        _modalBase.Close();
    }

    private List<LevelSelectGroup> _listSelectGroups = new List<LevelSelectGroup>();
}
