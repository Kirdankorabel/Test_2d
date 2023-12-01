using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private List<AudioClip> _clips;
    private int _counter = 0;

    void Awake()
    {
        SettingsPanel.Instance.OnMusicVolumeChanged += (value) => _musicAudioSource.volume = value;
    }

    private void Start()
    {
        StartCoroutine(PlayNextCorutine());
    }

    private IEnumerator PlayNextCorutine()
    {
        _musicAudioSource.clip = _clips[_counter];
        _musicAudioSource.Play();
        yield return new WaitForSeconds(_clips[_counter].length);
        _counter++;
        if (_counter == _clips.Count)
            _counter = 0;
        yield return StartCoroutine(PlayNextCorutine());
    }
}
