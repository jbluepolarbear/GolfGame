using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySpinner : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _SpinSpeed;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody)
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void FixedUpdate()
    {
        _rigidbody.angularVelocity = _SpinSpeed;
    }
}
