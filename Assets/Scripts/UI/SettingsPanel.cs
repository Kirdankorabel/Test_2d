using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Singleton<SettingsPanel>
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private string _soundVolumePrefsName = "volumeS";
    [SerializeField] private string _musicVolumePrefsName = "volumeM";

    public event System.Action<float> OnMusicVolumeChanged;
    public event System.Action<float> OnSoundVolumeChanged;

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseSettingsPanel);
        _soundVolumeSlider.onValueChanged.AddListener((value) => OnSoundVolumeChanged(value));
        _musicVolumeSlider.onValueChanged.AddListener((value) => OnMusicVolumeChanged(value));
    }

    private void Start()
    {
        _soundVolumeSlider.value = PlayerPrefs.GetFloat(_soundVolumePrefsName, 1);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat(_musicVolumePrefsName, 1);
        gameObject.SetActive(false);
    }

    private void CloseSettingsPanel()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetFloat(_soundVolumePrefsName, _soundVolumeSlider.value);
        PlayerPrefs.SetFloat(_musicVolumePrefsName, _musicVolumeSlider.value);
    }
}
