using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isActive = true;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(1);
        if (_isActive)
        {
            _audioSource.Play();
            _isActive = false;
        }
    }
}