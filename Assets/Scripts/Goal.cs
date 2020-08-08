using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private Level _level = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _level.GoalTriggered();
    }
}
