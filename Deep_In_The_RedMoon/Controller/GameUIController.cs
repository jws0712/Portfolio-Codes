namespace OTO.Controller
{
    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using UnityEngine.Audio;

    //TMP
    using TMPro;

    //System
    using OTO.Manager;
    using OTO.Charactor.Player;

    public class GameUIController : MonoBehaviour, IObserver
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI coinText = null;
        [SerializeField] private Slider hpSlider = null;
        [SerializeField] private TextMeshProUGUI ammoText = null;
        [Space(10)]
        [SerializeField] private GameObject waveText = null;
        [SerializeField] private TextMeshProUGUI waveNumber = null;
        [Space(10)]
        [SerializeField] private GameObject gameOverPanel = null;
        [SerializeField] private Button gameOverPanelRePlayButton = null;
        [SerializeField] private Button gameOverPanelTitleButton = null;
        [Space(10)]
        [SerializeField] private GameObject gameClearPanel = null;
        [SerializeField] private Button gameClearPanelKeepPlayButton = null;
        [SerializeField] private Button gameClearPanelTitleButton = null;

        [Space(10)]
        [SerializeField] private GameObject settingBackGround = null;
        [SerializeField] private GameObject settingPanel = null;
        [SerializeField] private GameObject audioSettingPanel = null;
        [SerializeField] private Button audioButton = null;


        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer = null;
        [SerializeField] private Slider musicSlider = null;
        [SerializeField] private Slider sfxSlider = null;

        //private variable
        private PlayerController playerController = null;

        private bool isToggleSettingPanel = default;
        private bool isToggleAudioPanel = default;

        private void Start()
        {
            GameManager.Instance.SetController(this);

            gameOverPanelRePlayButton.onClick.AddListener(RePlayButton);
            gameOverPanelTitleButton.onClick.AddListener(TitleButton);

            gameClearPanelKeepPlayButton.onClick.AddListener(KeepPlayButton);;
            gameClearPanelTitleButton.onClick.AddListener(TitleButton);
            audioButton.onClick.AddListener(AudioButton);

            StageEventBus.Subscribe(StageEventType.WaveStart, ToggleWaveText);
            StageEventBus.Subscribe(StageEventType.WaveClear, ToggleWaveText);
            StageEventBus.Subscribe(StageEventType.GameOver, ToggleGmaeOverPanel);
            StageEventBus.Subscribe(StageEventType.GameClear, ToggleGmaeClearPanel);

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
            if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameClear) return;

            gameOverPanel.SetActive(false);
            gameClearPanel.SetActive(false);
            ToggleSettingPanel();
        }

        //���̺� �ؽ�Ʈ�� ��� �ϴ� �Լ�
        private void ToggleWaveText()
        {
            waveText.SetActive(!waveText.activeSelf);
            waveNumber.text = (GameManager.Instance.CurrentWaveCount + 1).ToString();
        }

        //�����ִ� �г��� ��� �ϴ� �Լ�
        private void ToggleSettingPanel()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                settingBackGround.SetActive(true);
                Time.timeScale = 0;

                if (isToggleAudioPanel)
                {
                    audioSettingPanel.SetActive(false);
                    settingPanel.SetActive(true);
                    isToggleAudioPanel = false;
                }

                if (isToggleSettingPanel)
                {
                    settingBackGround.SetActive(false);
                    settingPanel.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }

        //����� ��ư�� ����� ������ �Լ�
        public void AudioButton()
        {
            settingPanel.SetActive(false);
            audioSettingPanel.SetActive(true);
            isToggleAudioPanel = true;
        }

        //���� ���� �г��� ��� �ϴ� �Լ�
        public void ToggleGmaeOverPanel()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        //���� Ŭ���� �г��� ��� �ϴ� �Լ�
        public void ToggleGmaeClearPanel()
        {
            gameClearPanel.SetActive(true);
            Time.timeScale = 0;
        }

        //���÷��� ��ư ������ �Լ�
        public void RePlayButton()
        {
            Scene scene = SceneManager.GetActiveScene();

            LoadingScreenController.LoadScene(scene.name);
        }

        //Ÿ��Ʋ��ư�� ����� ������ �Լ�
        public void TitleButton()
        {
            GameManager.Instance.InitScene();
            SceneManager.LoadScene("MainTitle");
        }

        //����ϱ� ��ư�� ����� �ϴ� �Լ�
        public void KeepPlayButton()
        {
            Scene scene = SceneManager.GetActiveScene();

            switch (scene.name)
            {
                case "Stage1":
                    {
                        GameManager.Instance.InitScene();
                        LoadingScreenController.LoadScene("Stage2");

                        break;
                    }
                case "Stage2":
                    {
                        GameManager.Instance.InitScene();
                        LoadingScreenController.LoadScene("Stage3");

                        break;
                    }
            }

        }

        //HpUI�� ���� ������Ʈ�ϴ� �Լ�
        private void UpdatePlayerHpBar(float currentHp, float maxHp)
        {
            hpSlider.value = currentHp / maxHp;

            if (currentHp < 0)
            {
                hpSlider.value = 0;
            }
        }

        //���� ���� ź���� ���� ������Ʈ�ϴ� �Լ�
        private void UpdateAmmoText(float currentAmmo)
        {
            Debug.Log("ź�� ������Ʈ");
            ammoText.text = currentAmmo.ToString() + " / ��";
        }

        private void UpdateCoinText(float currentCoin)
        {
            coinText.text = currentCoin.ToString();
        }

        public void Notify(Subject subject)
        {
            if (!playerController)
            {
                playerController = subject.GetComponent<PlayerController>();
                UpdateAmmoText(playerController.CurrentAmmo);
            }
            else
            {
                UpdatePlayerHpBar(playerController.PlayerManager.CurrentHp, playerController.PlayerManager.MaxHp);
                UpdateAmmoText(playerController.CurrentAmmo);
                UpdateCoinText(playerController.CurrentCoin);
            }
        }
    }

}