namespace OTO.Charactor.Monster
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Charactor.Player;
    using OTO.Manager;
    using OTO.Controller;

    public class Monster : Character
    {
        #region variable
        [Header("MonsterInfo")]
        [SerializeField] protected MonsterData monsterData = null;

        [Header("DropItem")]
        [SerializeField] private GameObject coinObject = null;

        //Protected variables
        protected Animator anim = null;
        protected Rigidbody2D rb = null;

        protected Transform playerTrasnform = null;
        protected Transform houseTransform = null;

        protected bool isChasePlayer = false;
        protected bool isAttack = false;
        protected bool isFlip = false;
        protected bool chaseHouse = false;

        protected float currentCoolTime = default;

        protected float moveSpeed = default;
        protected float monsterScale = default;

        protected float attackDamage = default;
        protected float bodyAttackDamage = default;
        protected float attackCoolTime = default;

        protected float chaseRange = default;
        protected float attackRange = default;
        protected float stopDistance = default;
        protected LayerMask chaseTarget = default;
        #endregion

        protected virtual void OnEnable()
        {
            Init();

            //이거 고쳐야함
            if(GameObject.FindGameObjectWithTag("House") != null)
            {
                houseTransform = GameObject.FindGameObjectWithTag("House").transform;
            }
            
            currentCoolTime = 0f;
        }

        protected virtual void Update()
        {
            CheckRange(chaseRange);
            CheackAttackRange(attackRange);

            if (isChasePlayer == true)
            {
                ChaseLogic(playerTrasnform);
            }
            else if(chaseHouse == true)
            {
                ChaseLogic(houseTransform);
            }
        }

        //몬스터 클래스의 필드를 초기화 하는 함수
        private void Init()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            maxHp = monsterData.MaxHp;
            moveSpeed = monsterData.MoveSpeed;
            monsterScale = monsterData.MonsterScale;

            attackDamage = monsterData.AttackDamage;
            bodyAttackDamage = monsterData.BodyAttackDamage;
            attackCoolTime = monsterData.AttackCoolTime;

            chaseRange = monsterData.ChaseRange;
            attackRange = monsterData.AttackRange;
            stopDistance = monsterData.StopDistance;
            chaseTarget = monsterData.CaseTarget;
        }

        //공격 쿨타임이 다 돌면 공격을 실행하는 함수
        protected virtual void Attack()
        {
            currentCoolTime += Time.deltaTime;

            if (currentCoolTime >= attackCoolTime)
            {
                isAttack = true;
            }
        }

        //집과 플레이어가 공격거리에 있는지 검사하는 함수
        private void CheackAttackRange(float range)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player") && isChasePlayer == true)
                {
                    Attack();
                }
                else if (collider.CompareTag("House") && isChasePlayer == false)
                {
                    Attack();
                }
            }
        }

        //사정거리 내에 플레이어가 있는지 판단하는 함수
        private void CheckRange(float range)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, range, chaseTarget);

            if (collider != null)
            {
                playerTrasnform = collider.transform;
                isChasePlayer = true;
            }
            else
            {
                isChasePlayer = false;
            }
        }

        //타겟으로 일정거리 다가가면 멈추는 함수
        private void ChaseLogic(Transform chaseTransform)
        {
            float objectDistance = Mathf.Abs(chaseTransform.position.x - transform.position.x);

            if (objectDistance < stopDistance)
            {
                StopChase();
            }
            else
            {
                Chase(chaseTransform);
            }
        }

        //타겟을 추적하는 함수
        private void Chase(Transform chaseTransform)
        {
            if (transform.position.x < chaseTransform.position.x)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector2(-monsterScale, monsterScale);
                isFlip = false;
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = new Vector2(monsterScale, monsterScale);
                isFlip = true;
            }
        }
        
        //몬스터를 멈추는 함수
        protected virtual void StopChase()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //몬스터가 죽었을때 아이템을 떨어뜨리는 함수
        private void DropItem(params GameObject[] dropItem)
        {
            for(int i = 0; i < dropItem.Length; i++)
            {
                GameObject item = ObjectPoolManager.Instance.GetPoolObject(dropItem[i]);
                item.transform.position = transform.position;

                item.GetComponent<Rigidbody2D>().AddForce(Vector2.one * -2f, ForceMode2D.Impulse);
            }
        }

        //데미지를 받을때 실행되는 함수
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        //몬스터가 죽었을때 실행되는 함수
        protected override void Die()
        {
            base.Die();

            GameManager.Instance.FieldMonsterCount--;

            if (GameManager.Instance.FieldMonsterCount == 0)
            {
                StageEventBus.Publish(StageEventType.WaveClear);
            }

            DropItem(coinObject);
            Destroy(gameObject);
        }

        //플레이어와 부딛쳤을때 플레이어에게 피해를 입힘
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<PlayerManager>().TakeDamage(bodyAttackDamage);
            }
        }

        //
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        collision.gameObject.GetComponent<PlayerManager>().TakeDamage(bodyAttackDamage);
        //    }
        //}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}