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

    public class House : MonoBehaviour, IDamageable
    {
        [Header("House Info")]
        [SerializeField] private float maxHp = default;
        [SerializeField] private Slider houseHpSlider = null;

        public float currentHp = default;

        private Animator anim = null;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            ResetHp();

            StageEventBus.Subscribe(StageEventType.Ready, OpenDoor);
            StageEventBus.Subscribe(StageEventType.WaveStart, CloseDoor);
        }

        // 체력UI와 체력을 관리하는 코드
        private void Update()
        {
            houseHpSlider.value = currentHp / maxHp;
        }

        //건물의 문을 여는 함수
        private void OpenDoor()
        {
            ResetHp();
            anim.SetBool("isOpen", true);
        }

        //건물의 문을 닫는 함수
        private void CloseDoor()
        {
            anim.SetBool("isOpen", false);
        }

        //건물의 체력을 초기화 하는 함수
        private void ResetHp()
        {
            currentHp = maxHp;
        }

        //데미지를 받을때 실행되는 함수
        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if(currentHp <= 0)
            {
                Die();
            }
        }

        //죽을때 실행되는 함수
        private void Die()
        {
            StageEventBus.Publish(StageEventType.GameOver);
        }
    }
}


