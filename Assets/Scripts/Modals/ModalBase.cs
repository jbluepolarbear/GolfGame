using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalBase : MonoBehaviour
{
    [SerializeField]
    private string _modalId;
    [SerializeField]
    private Animator _animator;

    public string ModalId => _modalId;

    public delegate void OnCloseCallback(string modalId);

    public void Disable_Modal()
    {
        gameObject.SetActive(false);
    }
    
    public void Open(OnCloseCallback closeCallback)
    {
        _closeCallback = closeCallback;
        _animator.SetBool("open", true);
    }
    
    public void Close()
    {
        _closeCallback?.Invoke(ModalId);
        _closeCallback = null;
        _animator.SetBool("open", false);
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(_modalId))
        {
            Debug.LogError("_modalID not set");
        }
    }

    private OnCloseCallback _closeCallback;
}
