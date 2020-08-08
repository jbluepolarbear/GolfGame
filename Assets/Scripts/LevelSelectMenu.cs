using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _levelSelect;
    private void OnEnable()
    {
        _levelSelect.SetActive(true);
    }

    private void OnDisable()
    {
        _levelSelect.SetActive(false);
    }
}
