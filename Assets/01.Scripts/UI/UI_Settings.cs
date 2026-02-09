using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        _returnButton.onClick.AddListener(Close);
        _exitButton.onClick.AddListener(QuitGame);
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseGame();
        LoadVolume();
    }

    private void OnDisable()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnDestroy()
    {
        _bgmVolumeSlider.onValueChanged.RemoveListener(SetBGMVolume);
        _sfxVolumeSlider.onValueChanged.RemoveListener(SetSfxVolume);
        _returnButton.onClick.RemoveListener(Close);
        _exitButton.onClick.RemoveListener(QuitGame);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
    
    private void LoadVolume()
    {
        _bgmVolumeSlider.value = SoundManager.Instance.GetBgmSound().Volume;
        _sfxVolumeSlider.value = SoundManager.Instance.GetSfxSound().Volume;
    }

    private void SetBGMVolume(float value)
    {
        SoundManager.Instance.SetBgmVolume(value);
    }

    private void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
