namespace OTO.Charactor.Monster
{
    using OTO.Manager;
    using OTO.Object;

    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    /// <summary>
    /// 보스의 상태
    /// </summary>
    public enum BatBossState
    {
        Idle,
        Skill1,
        Skill2,
        Skill3,
    }

    /// <summary>
    /// 박쥐 보스의 기능을 구현한 클래스
    /// </summary>
    public class BatBoss : Character
    {   
        private BatBossState batBossState;

        [Header("BatBoss Info")]
        [SerializeField] private float monsterScale = default;
        [SerializeField] private float attackDamage = default;
        [SerializeField] private float bodyAttackDamage = default;

        [Header("Skill")]
        [SerializeField] private GameObject bulletObject = null;
        [SerializeField] private GameObject fireObject = null;
        [SerializeField] private Transform shotPos = null;

        //private variables
        private Transform playerPos = default;
        private Rigidbody2D rb = null;
        private Animator anim = null;

        private bool isFlip = false;
        

        /// <summary>
        /// 변수 초기화
        /// </summary>
        private void OnEnable()
        {
            batBossState = BatBossState.Idle;
            anim.SetTrigger("Smoke");

            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// 플레이어의 위치에 따라 플립하는 코드
        /// </summary>
        private void Update()
        {
            if(GameObject.FindGameObjectWithTag("Player") != null)
            {
                playerPos = GameObject.FindGameObjectWithTag("Player").transform;

                if (transform.position.x < playerPos.position.x)
                {
                    transform.localScale = new Vector2(-monsterScale, monsterScale);
                    isFlip = false;
                }
                else
                {
                    transform.localScale = new Vector2(monsterScale, monsterScale);
                    isFlip = true;
                }
            }

            rb.velocity = Vector2.down;
        }

        /// <summary>
        /// 박쥐보스가 사라질때 효과음을 재생하는 함수
        /// </summary>
        public void PlaySmokeSFX()
        {
            AudioManager.Instance.PlaySFX("BatBossSmoke");
        }

        /// <summary>
        /// 보스의 행동 상태를 결정하는 함수
        /// </summary>
        public void ChangeState()
        {

            switch(batBossState)
            {
                case BatBossState.Idle:
                    {
                        Idle();
                        break;
                    }
                case BatBossState.Skill1:
                    {
                        Skill1();
                        break;
                    }
                case BatBossState.Skill2:
                    {
                        Skill2();
                        break;
                    }
            }
        }

        /// <summary>
        /// 기본 상태일때 실행되는 함수
        /// </summary>
        private void Idle()
        {
            transform.position = new Vector3(0, 0.3463205f, 0);
            StartCoroutine(Co_Idle());
        }

        /// <summary>
        /// 기본 상태일때 실행되는 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_Idle()
        {
            anim.SetTrigger("Idle");
            yield return new WaitForSeconds(3);
            int skillIndex = Random.Range(1, 3);

            switch (skillIndex)
            {
                case 1:
                    {
                        batBossState = BatBossState.Skill1;
                        break;
                    }
                case 2:
                    {
                        batBossState = BatBossState.Skill2;
                        break;
                    }
            }

            anim.SetTrigger("Smoke");


        }

        /// <summary>
        /// 검기를 날리는 스킬을 실행하는 함수
        /// </summary>
        private void Skill1()
        {
            StartCoroutine(Co_Skill1());
        }

        /// <summary>
        /// 검기를 날리는 로직을 구현한 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_Skill1()
        {
            transform.position = new Vector2(playerPos.position.x + 15, 0.3463205f);

            anim.SetTrigger("Skill1");

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1.5f);
                anim.SetTrigger("Skill1_Attack");
            }

            batBossState = BatBossState.Idle;
            anim.SetTrigger("Smoke");

        }

        /// <summary>
        /// 검기를 날리는 함수
        /// </summary>
        public void Skill1_Attack()
        {
            AudioManager.Instance.PlaySFX("BatBossAttack");
            GameObject bullet = Instantiate(bulletObject, shotPos.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().BulletDamage = attackDamage;

            if (isFlip)
            {
                //bullet.GetComponent<Bullet>().bulletSpeed *= -1;
            }
            else
            {
                bullet.transform.localScale = new Vector3(-bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
            }

            anim.SetTrigger("Skill1_Idle");
        }

        /// <summary>
        /// 불기둥을 스킬을 실행하는 함수
        /// </summary>
        private void Skill2()
        {
            anim.SetTrigger("Skill2");

            transform.position = new Vector2(0, 10.35f);
            StartCoroutine(Co_Skill2());
        }

        /// <summary>
        /// 불기둥을 소환 시키는 함수
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_Skill2()
        {
            yield return new WaitForSeconds(2);
            int fireCount = Random.Range(1, 4);

            for(int i = 0; i < fireCount; i++)
            {
                AudioManager.Instance.PlaySFX("BatBossFire");
                for (int j = 0; j < 10; j++)
                {
                    Instantiate(fireObject, new Vector3(50 - 10 * j, 2f, playerPos.position.z), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.5f);
                AudioManager.Instance.PlaySFX("BatBossFire");
                for (int k = 0; k < 10; k++)
                {
                    Instantiate(fireObject, new Vector3(45 - 10 * k, 2f, playerPos.position.z), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.5f);
            }

            batBossState = BatBossState.Idle;
            anim.SetTrigger("Smoke");
        }
    }
}
