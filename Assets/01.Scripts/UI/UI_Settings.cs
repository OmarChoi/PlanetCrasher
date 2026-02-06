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
        _bgmVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
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

    private void Close()
    {
        gameObject.SetActive(false);
    }
    
    private void LoadVolume()
    {
        // todo: 도메인을 반환해야한다.
        _bgmVolumeSlider.value = SoundManager.Instance.GetBgmVolume();
        _sfxVolumeSlider.value = SoundManager.Instance.GetSfxVolume();
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
