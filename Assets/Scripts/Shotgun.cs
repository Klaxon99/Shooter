using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _shootDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _impactForce;
    [SerializeField] private Transform _bulletEffect;
    [SerializeField] private float _bulletEffectOffset;
    [SerializeField] private ShootEffect _shootEffect;
    [SerializeField] private Animator _animator;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private AudioSource _reloadAudio;

    [Header("Shell")]
    [SerializeField] private Rigidbody _shellPrefab;
    [SerializeField] private Transform _shellPoint;
    [SerializeField] private float _shellSpeed;
    [SerializeField] private float _shellAngularSpeed;

    public void Shoot(Vector3 startPoint, Vector3 direction)
    {
        _shootEffect.Perform();
        _animator.SetTrigger("Shoot");
        _cameraShake.MakeRecoil();
        ExtractShell();

        if (Physics.Raycast(startPoint, direction, out RaycastHit hitInfo, _shootDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {

            Transform decal = Instantiate(_bulletEffect, hitInfo.transform);
            decal.position = hitInfo.point + hitInfo.normal * _bulletEffectOffset;
            decal.LookAt(hitInfo.point);
            decal.Rotate(Vector3.up * 180f);

            if (hitInfo.collider.TryGetComponent(out AbstractHealth health))
            {
                health.TakeDamage(_damage);

                Rigidbody rigidbody = hitInfo.collider.attachedRigidbody;

                if (rigidbody != null)
                {
                    hitInfo.rigidbody.AddForceAtPosition(direction * _impactForce, hitInfo.point);
                }
            }
        }
    }

    private void ExtractShell()
    {
        Rigidbody shell = Instantiate(_shellPrefab, _shellPoint.position, _shellPoint.rotation);

        shell.velocity = _shellPoint.forward * _shellSpeed;
        shell.angularVelocity = Vector3.up * _shellAngularSpeed;
    }

    public void PlayReloadSound()
    {
        _reloadAudio.Play();
    }
}