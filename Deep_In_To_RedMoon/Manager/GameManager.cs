namespace OTO.Manager
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.SceneManagement;

    //TMP
    using TMPro;

    //Projects
    using OTO.Charactor.Player;
    using OTO.Controller;

    public class GameManager : MonoSingleton<GameManager>
    {
        #region variable
        //private variable
        private int currentWaveCount = default;

        private float coinCount = default;
        private float fieldMonsterCount = default;

        private bool isGameOver = default;
        private bool isGameClear = default;

        private PlayerController playerController = null;
        private CameraController cameraController = null;
        private GameUIController gameUIController = null;

        private Transform houseTransform = null;

        //property
        public float CoinCount => coinCount;
        public bool IsGameOver => isGameOver;
        public bool IsGameClear => isGameClear;


        public Transform HouseTransform => houseTransform;
        public PlayerController PlayerController => playerController;
        public CameraController CameraController => cameraController;
        public GameUIController GameUIController => gameUIController;

        public float FieldMonsterCount { get { return fieldMonsterCount; } set { fieldMonsterCount = value; } }
        public int CurrentWaveCount { get { return currentWaveCount; } set { currentWaveCount = value; } }
        #endregion

        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            StageEventBus.Subscribe(StageEventType.StartStage, GameStart);
            StageEventBus.Subscribe(StageEventType.GameClear, GameClear);
            StageEventBus.Subscribe(StageEventType.GameOver, GameOver);
        }

        //컨트롤러를 가져오는 함수
        #region SetController
        public void SetController(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void SetController(CameraController cameraController)
        {
            this.cameraController = cameraController;
        }

        public void SetController(GameUIController gameUIController)
        {
            this.gameUIController = gameUIController;
        }
        #endregion

        //건물의 위치를 가져오는 함수
        public void SetHouseTarget(Transform houseTransform)
        {
            this.houseTransform = houseTransform;
        }

        //스테이지를 초기화 함수
        public void InitScene()
        {
            Time.timeScale = 1;

            isGameClear = false;
            isGameOver = false;

            FieldMonsterCount = 0;
            CurrentWaveCount = 0;
        }

        //게임이 시작할때 실행되는 함수
        private void GameStart()
        {
            InitScene();
            StageEventBus.Publish(StageEventType.Ready);
        }

        //게임 오버가 됬을때 실행되는 함수
        private void GameOver()
        {
            AudioManager.Instance.PlayMusic("GameOver");
            AudioManager.Instance.PlaySFX("GameOver");
            isGameOver = true;
        }

        //게임을 클리어 했을때 실행되는 함수
        private void GameClear()
        {
            isGameClear = true;
        }

        public void OnMonsterDefeated()
        {
            fieldMonsterCount--;

            if(fieldMonsterCount == 0)
            {
                StageEventBus.Publish(StageEventType.WaveClear);
            }
        }

        //현재 씬을 검사해서 음악을 실행하는 함수
        public void PlaySceneMusic()
        {
            Scene scene = SceneManager.GetActiveScene();
            switch (scene.name)
            {
                case "MainTitle":
                    {
                        AudioManager.Instance.PlayMusic("Title");
                        break;
                    }
                case "Stage1":
                    {
                        AudioManager.Instance.PlayMusic("Stage1");
                        break;
                    }

                case "Stage2":
                    {
                        AudioManager.Instance.PlayMusic("Stage2");
                        break;
                    }
                case "Stage3":
                    {
                        AudioManager.Instance.PlayMusic("Stage3");
                        break;
                    }
            }
        }
    }
}