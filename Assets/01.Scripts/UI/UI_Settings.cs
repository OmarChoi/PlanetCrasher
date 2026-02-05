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
        _returnButton.onClick.AddListener(() => gameObject.SetActive(false));
        _exitButton.onClick.AddListener(QuitGame);
    }
    
    private void OnEnable()
    {
        LoadVolume();
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        SetVolume();
        Time.timeScale = 1f;
    }
    
    private void LoadVolume()
    {
        _bgmVolumeSlider.value = SoundManager.Instance.GetBgmVolume();
        _sfxVolumeSlider.value = SoundManager.Instance.GetSfxVolume();
    }

    private void SetVolume()
    {
        float bgmVolume = _bgmVolumeSlider.value;
        float sfxVolume = _sfxVolumeSlider.value;
        SoundManager.Instance.SetVolume(bgmVolume, sfxVolume);
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
