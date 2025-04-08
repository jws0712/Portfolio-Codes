namespace OTO.Controller
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Audio;

    //Project
    using OTO.Manager;

    public class TitleUIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject settingBackGround = null;
        [SerializeField] private GameObject settingPanel = null;
        [SerializeField] private GameObject audioSettingPanel = null;
        [SerializeField] private GameObject tipPanel = null;
        [SerializeField] private GameObject creditPanel = null;
        [Space(10)]
        [SerializeField] private Button startButton = null;
        [SerializeField] private Button settingButton = null;
        [SerializeField] private Button creditButton = null;
        [SerializeField] private Button tipButton = null;
        [SerializeField] private Button quitButton = null;
        [SerializeField] private Button audioButton = null;
        [SerializeField] private GameObject buttonGroupObject = null;
        [Space(10)]
        [SerializeField] private AudioMixer audioMixer = null;
        [SerializeField] private Slider musicSlider = null;
        [SerializeField] private Slider sfxSlider = null;

        //private variable
        private bool isTogglePanel = default;
        private bool isToggleAudioPanel = default;

        private void Start()
        {
            Cursor.visible = true;

            startButton.onClick.AddListener(StartButton);
            settingButton.onClick.AddListener(() => { ToggleSettingPanel(true); });
            creditButton.onClick.AddListener(() => { ToggleCreditPanel(true); });
            tipButton.onClick.AddListener(() => { ToggleTipPanel(true); });
            audioButton.onClick.AddListener(() => { ToggleAudioPanel(true); });
            quitButton.onClick.AddListener(QuitButton);

            musicSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetMusicVolume(value, audioMixer));
            sfxSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetSFXVolume(value, audioMixer));


            if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
            {
                AudioManager.Instance.LoadVolume(musicSlider.value, sfxSlider.value, audioMixer);

                musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

            }
            else
            {
                AudioManager.Instance.SetMusicVolume(musicSlider.value, audioMixer);
                AudioManager.Instance.SetSFXVolume(sfxSlider.value, audioMixer);
            }

            GameManager.Instance.PlaySceneMusic();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(isToggleAudioPanel)
                {
                    ToggleAudioPanel(false);
                    return;
                }

                if(isTogglePanel)
                {
                    ToggleSettingPanel(false);
                    ToggleAudioPanel(false);
                    ToggleCreditPanel(false);
                    ToggleTipPanel(false);
                }
                else
                {
                    ToggleSettingPanel(true);
                }
            }

            buttonGroupObject.SetActive(!isTogglePanel);
        }

        //시작버튼의 기능을 구현한 함수
        public void StartButton()
        {
            AudioManager.Instance.PlaySFX("PressStartButton");

            LoadingScreenController.LoadScene("Stage1");
        }

        // 나가기 버튼의 기능을 구현한 함수
        public void QuitButton()
        {
            Application.Quit();
        }

        //세팅 패널을 토글하는 함수
        public void ToggleSettingPanel(bool isActive)
        {
            settingPanel.SetActive(isActive);
            settingBackGround.SetActive(isActive);
            isTogglePanel = isActive;
        }

        //오디오 패널을 토글하는 함수
        public void ToggleAudioPanel(bool isActive)
        {
            settingPanel.SetActive(isActive);
            audioSettingPanel.SetActive(isActive);
            isToggleAudioPanel = isActive;
        }

        //크래딧 패널을 토글하는 함수
        public void ToggleCreditPanel(bool isActive)
        {
            creditPanel.SetActive(isActive);
            isTogglePanel = isActive;

        }

        //팁 패널을 토글하는 함수
        public void ToggleTipPanel(bool isActive)
        {
            tipPanel.SetActive(isActive);
            isTogglePanel = isActive;
        }
    }
}
