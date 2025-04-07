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


        private bool isToggleSettingPanel = default;
        private bool isToggleAudioPanel = default;

        private void Start()
        {
            Cursor.visible = true;

            startButton.onClick.AddListener(StartButton);
            settingButton.onClick.AddListener(SettingButton);
            creditButton.onClick.AddListener(CreditButton);
            tipButton.onClick.AddListener(TipButton);
            quitButton.onClick.AddListener(QuitButton);
            audioButton.onClick.AddListener(AudioButton);

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
        }

        private void Update()
        {
            ToggleSettingPanel();
        }

        //���� �г��� ����ϴ� �Լ�
        private void ToggleSettingPanel()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && isToggleSettingPanel)
            {
                settingBackGround.SetActive(false);
                settingPanel.SetActive(false);
                tipPanel.SetActive(false);
                creditPanel.SetActive(false);

                buttonGroupObject.SetActive(true);

                if(isToggleAudioPanel)
                {
                    audioSettingPanel.SetActive(false);
                    settingPanel.SetActive(true);
                    isToggleAudioPanel = false;
                }
            }
        }

        //���۹�ư�� ����� ������ �Լ�
        public void StartButton()
        {
            AudioManager.Instance.PlaySFX("PressStartButton");

            LoadingScreenController.LoadScene("Stage1");
        }

        //����� ��ư�� ����� ������ �Լ�
        public void AudioButton()
        {
            buttonGroupObject.SetActive(false);
            settingPanel.SetActive(false);
            audioSettingPanel.SetActive(true);
            buttonGroupObject.SetActive(false);
            isToggleAudioPanel = true;
        }

        // ���� ��ư�� ����� ������ �Լ�
        public void SettingButton()
        {
            settingBackGround.SetActive(true);
            settingPanel.SetActive(true);
            buttonGroupObject.SetActive(false);
            isToggleSettingPanel = true;
        }

        //ũ���� ��ư�� ����� ������ �Լ�
        public void CreditButton()
        {
            creditPanel.SetActive(true);
            buttonGroupObject.SetActive(false);
            isToggleSettingPanel = true;

        }

        //���� ����� ������ �Լ�
        public void TipButton()
        {
            tipPanel.SetActive(true);
            buttonGroupObject.SetActive(false);
            isToggleSettingPanel = true;
        }

        // ������ ��ư�� ����� ������ �Լ�
        public void QuitButton()
        {
            Application.Quit();
        }
    }
}
