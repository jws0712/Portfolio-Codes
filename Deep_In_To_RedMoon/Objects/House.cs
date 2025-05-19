namespace OTO.Object 
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //Project
    using OTO.Controller;
    using OTO.Manager;

    public class House : MonoBehaviour, IDamageable
    {
        [Header("House Info")]
        [SerializeField] private float maxHp = default;
        [SerializeField] private Slider houseHpSlider = null;

        private float currentHp = default;

        private Animator anim = null;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            GameManager.Instance.SetHouseTarget(transform);

            ResetHp();

            StageEventBus.Subscribe(StageEventType.Ready, OpenDoor);
            StageEventBus.Subscribe(StageEventType.WaveStart, CloseDoor);
        }

        // 체력UI와 체력을 관리
        private void Update()
        {
            houseHpSlider.value = currentHp / maxHp;
        }

        //문여는 애니매이션 실행
        private void OpenDoor()
        {
            ResetHp();
            anim.SetBool("isOpen", true);
        }

        //문닫는 애니매이션 실행
        private void CloseDoor()
        {
            anim.SetBool("isOpen", false);
        }

        //건물의 체력을 초기화
        private void ResetHp()
        {
            currentHp = maxHp;
        }

        //데미지를 받을때 실행
        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if(currentHp <= 0)
            {
                Die();
            }
        }

        //사망
        private void Die()
        {
            StageEventBus.Publish(StageEventType.GameOver);
        }
    }
}


