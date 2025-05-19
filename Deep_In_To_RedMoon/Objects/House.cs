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

        // ü��UI�� ü���� ����
        private void Update()
        {
            houseHpSlider.value = currentHp / maxHp;
        }

        //������ �ִϸ��̼� ����
        private void OpenDoor()
        {
            ResetHp();
            anim.SetBool("isOpen", true);
        }

        //���ݴ� �ִϸ��̼� ����
        private void CloseDoor()
        {
            anim.SetBool("isOpen", false);
        }

        //�ǹ��� ü���� �ʱ�ȭ
        private void ResetHp()
        {
            currentHp = maxHp;
        }

        //�������� ������ ����
        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if(currentHp <= 0)
            {
                Die();
            }
        }

        //���
        private void Die()
        {
            StageEventBus.Publish(StageEventType.GameOver);
        }
    }
}


