using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private SoundManager _soundManager;
    private GameManager _gameManager;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _soundManager = SoundManager.Instance;
        _bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        _bgmVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        _returnButton.onClick.AddListener(Close);
        _exitButton.onClick.AddListener(QuitGame);
    }

    private void OnEnable()
    {
        _gameManager.PauseGame();
        LoadVolume();
    }

    private void OnDisable()
    {
        _gameManager.ResumeGame();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
    
    private void LoadVolume()
    {
        _bgmVolumeSlider.value = _soundManager.GetBgmVolume();
        _sfxVolumeSlider.value = _soundManager.GetSfxVolume();
    }

    private void SetBGMVolume(float value)
    {
        _soundManager.SetBgmVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        _soundManager.SetSfxVolume(value);
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
