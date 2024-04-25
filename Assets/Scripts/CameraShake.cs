using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    [Header("Shake")]
    [SerializeField] private float _perlinNoiseTimeScale;
    [SerializeField] private AnimationCurve _perlinNoiseCurve;

    [Header("Recoil")]
    [SerializeField] private float _tension;
    [SerializeField] private float _damping;
    [SerializeField] private float _impuls;

    private float _shakeTimer = 0f;
    private float _duration = 0f;
    private float _amplitude = 0f;
    private Vector3 _recoilVelocity = Vector3.zero;
    private Vector3 _shakeAngles = Vector3.zero;
    private Vector3 _recoilAngles = Vector3.zero;

    private void Update()
    {
        UpdateRecoil();
        UpdateShake();

        _cameraTransform.localEulerAngles = _shakeAngles + _recoilAngles;
    }

    private void UpdateRecoil()
    {
        _recoilAngles += _recoilVelocity * Time.deltaTime;
        _recoilVelocity += -_recoilAngles * _tension * Time.deltaTime;
        _recoilVelocity = Vector3.Lerp(_recoilVelocity, Vector3.zero, _damping * Time.deltaTime);
    }

    private void UpdateShake()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime / _duration;
        }

        float time = Time.time * _perlinNoiseTimeScale;
        _shakeAngles.x = Mathf.PerlinNoise(time, 0);
        _shakeAngles.y = Mathf.PerlinNoise(0, time);
        _shakeAngles.z = Mathf.PerlinNoise(time, time);

        _shakeAngles *= _amplitude;
        _shakeAngles *= _perlinNoiseCurve.Evaluate(Mathf.Clamp01(1 - _shakeTimer));
    }

    public void MakeRecoil()
    {
        MakeRecoil(-Vector3.right * Random.Range(_impuls * 0.5f, _impuls));
    }

    public void MakeRecoil(Vector3 impulse)
    {
        _recoilVelocity += impulse;
    }

    public void MakeShake(float amplitude, float duration)
    {
        _shakeTimer = 1f;
        _duration = Mathf.Max(duration, 0.05f);
        _amplitude = amplitude;
    }
}