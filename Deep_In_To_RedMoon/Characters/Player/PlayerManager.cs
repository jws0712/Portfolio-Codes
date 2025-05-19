namespace OTO.Charactor.Player
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;
    using OTO.Controller;

    public class PlayerManager : Character
    {
        #region variables
        [Header("GetHit")]
        [SerializeField] private float playerFlashCount = default;
        [SerializeField] private float duration = default;

        //private variables
        private GameObject gunObject = null;
        private GameObject handPos = null;

        private SpriteRenderer gunRenderer = null;

        private PlayerController playerController = null;

        private LayerMask monsterLayer = default;
        private LayerMask playerLayer = default;

        //property
        public float CurrentHp => currentHp;
        public float MaxHp => maxHp;
        #endregion

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }
        private void OnEnable()
        {
            StageEventBus.Publish(StageEventType.StartStage);
            StageEventBus.Subscribe(StageEventType.Ready, ResetHp);
        }

        protected override void Start()
        {
            base.Start();

            monsterLayer = LayerMask.NameToLayer("Monster");
            playerLayer = LayerMask.NameToLayer("Player");

            Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, false);

        }

        //플레이어의 체력을 초기화
        private void ResetHp()
        {
            currentHp = maxHp;
        }

        //데미지를 받을때 실행
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            playerController.CameraShakeType = "Hit";
            playerController.NotifyObservers();

            handPos = GameObject.FindWithTag("HandPos");

            if(handPos != null)
            {
                gunObject = handPos.transform.GetChild(0).gameObject;
            }

            if (gunObject != null)
            {
                gunRenderer = gunObject.GetComponentInChildren<SpriteRenderer>();
            }

            StartCoroutine(Co_PlayerSpriteFlash(playerFlashCount));
        }

        //스프라이트를 깜빡거리게 함
        private IEnumerator Co_PlayerSpriteFlash(float Count)
        {
            Color _playerAlpha = renderer.color;
            for (int i = 0; i < Count; i++)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, true);
                yield return new WaitForSeconds(0.05f);
                _playerAlpha.a = 0f;
                renderer.color = _playerAlpha;
                gunRenderer.color = _playerAlpha;
                yield return new WaitForSeconds(duration);
                _playerAlpha.a = 1f;
                renderer.color = _playerAlpha;
                gunRenderer.color = _playerAlpha;
                yield return new WaitForSeconds(duration);
            }
            Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, false);
        }

        //사망
        protected override void Die()
        {
            base.Die();
            playerController.NotifyObservers();
            StageEventBus.Publish(StageEventType.GameOver);
            Destroy(gameObject);
        }
    }
}
