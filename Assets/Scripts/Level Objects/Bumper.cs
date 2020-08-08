using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    protected float _thrust = 100.0f;
    
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private AudioSource _audioSource;

    private void OnValidate()
    {
        _animation = GetComponent<Animation>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector3 point = other.contacts[0].point;
        Vector3 force = GetForce(point);
        other.gameObject.GetComponent<Rigidbody2D>()?.AddForce(force);
        if (_animation != null)
        {
            _animation.Play();
        }

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    protected virtual Vector3 GetForce(Vector3 otherPosition)
    {
        Vector3 vecA = new Vector3(otherPosition.x, otherPosition.y, 0.0f);
        Vector3 vecB = new Vector3(transform.position.x, transform.position.y, 0.0f);
        return (vecA - vecB).normalized * _thrust;
    }
}
