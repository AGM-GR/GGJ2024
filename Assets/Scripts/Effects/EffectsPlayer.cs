using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPlayer : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private ParticleSystem[] _particlesList;

    [SerializeField] private GameObject[] _objectsToEnable;

    [SerializeField] private AudioSource[] _soundsList;

    public string Id => _id; 

    public void Play()
    {
        if (_particlesList != null)
        {
            foreach (ParticleSystem particles in _particlesList)
            {
                if (particles != null)
                {
                    particles.Play();
                }
            }
        }

        if (_objectsToEnable != null)
        {
            foreach (GameObject toEnable in _objectsToEnable)
            {
                if (toEnable != null)
                {
                    toEnable.SetActive(false);
                    toEnable.SetActive(true);
                }
            }
        }

        if (_soundsList != null)
        {
            foreach (AudioSource sound in _soundsList)
            {
                if (sound != null)
                {
                    sound.Play();
                }
            }
        }
    }

    public void Stop()
    {
        if (_particlesList != null)
        {
            foreach (ParticleSystem particles in _particlesList)
            {
                if (particles != null)
                {
                    particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }

        if (_objectsToEnable != null)
        {
            foreach (GameObject toEnable in _objectsToEnable)
            {
                if (toEnable != null)
                {
                    toEnable.SetActive(false);
                }
            }
        }

        if (_soundsList != null)
        {
            foreach (AudioSource sound in _soundsList)
            {
                if (sound != null)
                {
                    sound.Stop();
                }
            }
        }
    }
}
