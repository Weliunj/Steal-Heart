using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [Header("Setting UI")]
    public GameObject settingPanel;
    public Button resumeButton;
    public Button menuButton;
    public Button quitButton;

    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;

    void Start()
    {
        // Khởi tạo UI
        if (settingPanel != null)
            settingPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (menuButton != null)
            menuButton.onClick.AddListener(BackToMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Thiết lập audio sliders
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Đảm bảo game chạy bình thường khi bắt đầu
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Cho phép mở setting bằng phím ESC (chỉ trên PC)
        if (Application.platform != RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSetting();
        }
    }

    public void ToggleSetting()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            // Tạm dừng game
            Time.timeScale = 0f;
            if (settingPanel != null)
                settingPanel.SetActive(true);
        }
        else
        {
            // Tiếp tục game
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        // Đảm bảo game tiếp tục trước khi chuyển scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Load scene menu (index 0)
    }

    public void QuitGame()
    {
        // Đảm bảo game tiếp tục trước khi quit
        Time.timeScale = 1f;
        
        #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        #else
            Application.Quit();
        #endif
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
        // Áp dụng volume cho tất cả AudioSource (background music)
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            // Chỉ áp dụng cho AudioSource có loop = true (thường là background music)
            if (source.loop)
            {
                source.volume = volume;
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        
        // Áp dụng volume cho tất cả AudioSource (sound effects)
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            // Chỉ áp dụng cho AudioSource có loop = false (thường là sound effects)
            if (!source.loop)
            {
                source.volume = volume;
            }
        }
    }

    // Hàm để kiểm tra trạng thái pause
    public bool IsGamePaused()
    {
        return isPaused;
    }
} 