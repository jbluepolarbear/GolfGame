using System;
using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _goalPoint;
    
    [Header("Design")]
    [SerializeField]
    private int _par = 3;
    [SerializeField]
    private int _maxTurns = 5;
    [SerializeField]
    private float _levelScale = 1.0f;
    [SerializeField]
    private float _maxThrust = 1000.0f;

    public Transform StartPoint => _startPoint.transform;
    public int MaxTurns => _maxTurns;
    public int Par => _par;
    public float LevelScale => _levelScale;
    public float MaxThrust => _maxThrust;

    public Transform GoalPoint => _goalPoint.transform;

    /// <summary>
    /// Called by goal when golf ball enters goal
    /// </summary>
    public void GoalTriggered()
    {
        Context.Get<GolfBall>().SetActing(false);
        Context.Get<GameController>()?.LevelSuccess();
        Debug.Log("Goal hit!");
    }

    /// <summary>
    /// Called by GameController to close level.
    /// </summary>
    public void Close()
    {
        _animator.SetBool("open", false);
    }

    /// <summary>
    /// Called by animation event
    /// </summary>
    public void CleanUp()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        GolfBall golfBall = Context.Get<GolfBall>();
        golfBall.transform.position = _startPoint.transform.position;
        golfBall.SetActing(true);
    }
}
