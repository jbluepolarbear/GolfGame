using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectGroup : MonoBehaviour
{
    [SerializeField]
    private LevelSelectItem[] _levelSelectItems;

    public void SetLevelSelectItem(int index, int score, int level, bool locked, LevelSelectItem.LevelSelectedCallback callback)
    {
        _levelSelectItems[index].gameObject.SetActive(true);
        _levelSelectItems[index].SetLevelItem(score, level, locked, callback);
    }

    public void DisableSelectItem(int index)
    {
        _levelSelectItems[index].gameObject.SetActive(false);
    }
}
