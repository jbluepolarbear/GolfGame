using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private bool _state;

    public bool State => _state;
    public void SetState(bool state)
    {
        _state = state;
        _animator.SetBool("off", !_state);
    }
}
