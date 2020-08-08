using System;
using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class AudioOnCollision : MonoBehaviour
{
    [SerializeField]
    private AudioSource _collisionAudioSource;

    private void OnValidate()
    {
        _collisionAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Context.Get<AudioManager>().PlaySound(_collisionAudioSource);
    }
}
