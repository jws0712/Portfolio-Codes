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
        #region 변수
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

        private bool isTogglePanel = default;
        private bool isToggleAudioPanel = default;
        #endregion

        private void Start()
        {
            GameManager.Instance.SetController(this);

            gameOverPanel.SetActive(false);
            gameClearPanel.SetActive(false);

            gameOverPanelRePlayButton.onClick.AddListener(RePlayButton);
            gameOverPanelTitleButton.onClick.AddListener(TitleButton);

            gameClearPanelKeepPlayButton.onClick.AddListener(KeepPlayButton);;
            gameClearPanelTitleButton.onClick.AddListener(TitleButton);
            audioButton.onClick.AddListener(() => { ToggleAudioPanel(true); });

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

            GameManager.Instance.PlaySceneMusic();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameClear) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isToggleAudioPanel)
                {
                    ToggleAudioPanel(false);
                    return;
                }

                if (isTogglePanel)
                {
                    ToggleSettingPanel(false);
                    Time.timeScale = 1;
                }
                else
                {
                    ToggleSettingPanel(true);
                    Time.timeScale = 0;
                }
            }
        }

        //웨이브 텍스트 토글
        private void ToggleWaveText()
        {
            waveText.SetActive(!waveText.activeSelf);
            waveNumber.text = (GameManager.Instance.CurrentWaveCount + 1).ToString();
        }

        //세팅 패널 토글
        public void ToggleSettingPanel(bool isActive)
        {
            settingPanel.SetActive(isActive);
            settingBackGround.SetActive(isActive);
            isTogglePanel = isActive;
        }

        //오디오 패널 토글
        public void ToggleAudioPanel(bool isActive)
        {
            settingPanel.SetActive(audioSettingPanel.activeSelf);
            audioSettingPanel.SetActive(!audioSettingPanel.activeSelf);
            isToggleAudioPanel = audioSettingPanel.activeSelf;
        }

        //게임 오버 패널 토글
        public void ToggleGmaeOverPanel()
        {
            gameOverPanel.SetActive(true);
            isTogglePanel = true;
        }

        //게임 클리어 패널 토글
        public void ToggleGmaeClearPanel()
        {
            gameClearPanel.SetActive(true);
            isTogglePanel = true;
        }

        public void RePlayButton()
        {
            Scene scene = SceneManager.GetActiveScene();

            LoadingScreenController.LoadScene(scene.name);
        }

        public void TitleButton()
        {
            GameManager.Instance.InitScene();
            SceneManager.LoadScene("MainTitle");
        }

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

        //HpUI의 값을 업데이트
        private void UpdatePlayerHpBar(float currentHp, float maxHp)
        {
            hpSlider.value = currentHp / maxHp;

            if (currentHp < 0)
            {
                hpSlider.value = 0;
            }
        }

        //현재 남은 탄약의 값을 업데이트
        private void UpdateAmmoText(float currentAmmo)
        {
            ammoText.text = currentAmmo.ToString() + " / ∞";
        }

        //현재 가지고 있는 코인의 개수를 업데이트
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