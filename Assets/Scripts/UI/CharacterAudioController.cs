using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private AudioSource _attackAudioSource;

    private bool _isPlaying;

    private void Awake()
    {
        SettingsPanel.Instance.OnSoundVolumeChanged += (value) => _stepAudioSource.volume = value;
        SettingsPanel.Instance.OnSoundVolumeChanged += (value) => _attackAudioSource.volume = value;
    }

    public void PlayStepAudio(bool play)
    {
        if (play && !_isPlaying)
        {
            _stepAudioSource.Play();
            _isPlaying = true;
        }
        else if (!play && _isPlaying)
        {
            _stepAudioSource.Stop();
            _isPlaying = false;
        }
    }
    
    public void PlayAttackAudio()
    {
        _attackAudioSource.Play();
    }
}
