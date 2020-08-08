using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GolfBall : ContextProvider<GolfBall>
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameObject _launch;

    [Header("Design")]
    [SerializeField]
    private float _stationaryToulerance = 1.0f;
    [SerializeField]
    private float _maxLaunchSize = 600.0f;
    
    
    private float _maxThrust = 1000.0f;

    public int TurnsTaken { get; private set; }

    public void LevelReset(Vector3 startPosition, float levelScale, float maxThrust)
    {
        SetActing(true);
        ResetTurnsTaken();
        _maxThrust = maxThrust;
        transform.position = startPosition;
        transform.localScale = new Vector3(levelScale, levelScale, 1.0f);
    }

    public void ResetTurnsTaken()
    {
        TurnsTaken = 0;
        transform.rotation = Quaternion.identity;
    }
    
    public void SetActing(bool acting)
    {
        if (acting)
        {
            _canvasGroup.alpha = 1.0f;
            Context.Get<InputController>().Interactable = true;
            _rigidbody.simulated = true;
            if (_animator)
            {
                _animator.SetBool("idle", true);
                _animator.SetBool("launching", false);
            }
        }
        else
        {
            _launch.transform.localScale = Vector3.zero;
            _physicsActing = false;
            Context.Get<InputController>().Interactable = false;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
            _rigidbody.Sleep();
            _canvasGroup.alpha = 0.0f;
        }
    }

    private void DraggingBegin()
    {
        _launch.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
        if (_animator)
        {
            _animator.SetBool("idle", false);
            _animator.SetBool("launching", true);
        }
        _rigidbody.velocity = Vector2.zero;
    }

    private void DraggingEnd()
    {
        if (_physicsActing)
        {
            return;
        }

        if (_animator)
        {
            _animator.SetBool("launching", false);
        }

        _launch.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        InputController inputController = Context.Get<InputController>();
        if (inputController.DragPercentage <= float.Epsilon)
        {
            if (_animator)
            {
                _animator.SetBool("idle", true);
            }
            return;
        }

        inputController.Interactable = false;

        TurnsTaken += 1;
        Vector2 forceToAdd = inputController.DragDirection * inputController.DragPercentage * _maxThrust;
        _rigidbody.AddForce(forceToAdd);

        StartCoroutine(DelayActingUpdate());
    }

    private IEnumerator DelayActingUpdate()
    {
        yield return new WaitForSeconds(0.25f);
        
        _physicsActing = true;
        if (_animator)
        {
            _animator.SetBool("launching", false);
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _launch.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.Sleep();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLaunch();
        UpdateActing();
    }

    private void OnEnable()
    {
        Context.Get<InputController>().DraggingBeginEvent += DraggingBegin;
        Context.Get<InputController>().DraggingEndEvent += DraggingEnd;
    }

    private void OnDisable()
    {
        Context.Get<InputController>().DraggingBeginEvent -= DraggingBegin;
        Context.Get<InputController>().DraggingEndEvent -= DraggingEnd;
    }

    private void UpdateLaunch()
    {
        InputController inputController = Context.Get<InputController>();
        if (!inputController.Dragging || inputController.DragPercentage <= float.Epsilon)
        {
            return;
        }

        _launch.transform.localScale = new Vector3(inputController.DragPercentage * _maxLaunchSize, 1.0f, 1.0f);
        Vector2 dragNormal = inputController.DragDirection.normalized;
        float angle = Mathf.Atan2(dragNormal.y, dragNormal.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
        _launch.transform.rotation = rotation;
    }
    
    private void UpdateActing()
    {
        if (!_physicsActing)
        {
            return;
        }

        if (_rigidbody.IsSleeping() ||
            _rigidbody.velocity.magnitude <= _stationaryToulerance)
        {
            Context.Get<InputController>().Interactable = true;
            _physicsActing = false;
            _rigidbody.velocity = Vector2.zero;

            if (_animator)
            {
                _animator.SetBool("idle", true);
            }

            Context.Get<GameController>()?.StarSettled();
        }
    }

    private bool _physicsActing = false;
}
