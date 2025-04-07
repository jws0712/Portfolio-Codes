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

        // ü��UI�� ü���� �����ϴ� �ڵ�
        private void Update()
        {
            houseHpSlider.value = currentHp / maxHp;
        }

        //�ǹ��� ���� ���� �Լ�
        private void OpenDoor()
        {
            ResetHp();
            anim.SetBool("isOpen", true);
        }

        //�ǹ��� ���� �ݴ� �Լ�
        private void CloseDoor()
        {
            anim.SetBool("isOpen", false);
        }

        //�ǹ��� ü���� �ʱ�ȭ �ϴ� �Լ�
        private void ResetHp()
        {
            currentHp = maxHp;
        }

        //�������� ������ ����Ǵ� �Լ�
        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if(currentHp <= 0)
            {
                Die();
            }
        }

        //������ ����Ǵ� �Լ�
        private void Die()
        {
            StageEventBus.Publish(StageEventType.GameOver);
        }
    }
}


