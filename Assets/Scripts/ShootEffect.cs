using System.Collections;
using UnityEngine;

public class ShootEffect : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _lightSource;

    public void Perform()
    {
        StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        _audioSource.Play();
        _particleSystem.Clear();
        _particleSystem.Play();
        _lightSource.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _lightSource.SetActive(false);
    }
}