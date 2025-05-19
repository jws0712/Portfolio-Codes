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

    //���� �ൿ Enum
    public enum MonsterState
    {
        ChasePlayer,
        ChaseHouse,
        Attack,
    }

    public abstract class Monster : Character
    {
        #region ����
        [Header("MonsterInfo")]
        [SerializeField] protected MonsterData monsterData = null;

        [Header("DropItem")]
        [SerializeField] protected GameObject[] coinObject = null;

        //protected variables
        protected Animator anim = null;
        protected Rigidbody2D rb = null;

        protected Transform playerTransform = null;
        protected Transform houseTransform = null;

        protected bool isChasePlayer = false;
        protected bool isAttack = false;
        protected bool isFlip = false;

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

        //private variables
        protected MonsterState monsterState = default;
        #endregion

        //�ʱ�ȭ
        protected virtual void OnEnable()
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

            houseTransform = GameManager.Instance.HouseTransform;
            
            currentCoolTime = 0f;
        }

        protected virtual void Update()
        {
            CheckRange(chaseRange);
            CheackAttackRange(attackRange, chaseTarget);

            switch(monsterState)
            {
                case MonsterState.ChaseHouse:
                    {
                        TargetChase(houseTransform);
                        isChasePlayer = false;
                        currentCoolTime = 0;
                        break;
                    }

                case MonsterState.ChasePlayer:
                    {
                        TargetChase(playerTransform);
                        isChasePlayer = true;
                        currentCoolTime = 0;
                        break;
                    }

                case MonsterState.Attack:
                    {
                        StopChase();
                        isAttack = false;
                        currentCoolTime += Time.deltaTime;

                        if(currentCoolTime >= attackCoolTime)
                        {
                            isAttack = true;
                            Attack();
                            Debug.Log("����");
                            currentCoolTime = 0;
                        }
                        break;
                    }
            }
        }

        //����
        protected abstract void Attack();

        //���� �÷��̾ ���ݰŸ��� �ִ��� �˻�
        private void CheackAttackRange(float range, LayerMask attackTarget)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, range, attackTarget);

            if(collider != null)
            {
                monsterState = MonsterState.Attack;
            }
        }

        //�����Ÿ� ���� �÷��̾ �ִ��� �˻�
        private void CheckRange(float range)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, range, chaseTarget);

            if (collider != null)
            {
                playerTransform = collider.transform;
                monsterState = MonsterState.ChasePlayer;
            }
            else
            {
                if(monsterState != MonsterState.Attack)
                {
                    monsterState = MonsterState.ChaseHouse;
                }
            }
        }

        //Ÿ�� ����
        private void TargetChase(Transform targetTransform)
        {
            float objectDistance = Mathf.Abs(targetTransform.position.x - transform.position.x);

            if (objectDistance < stopDistance)
            {
                monsterState = MonsterState.Attack;
            }
            else
            {
                if (transform.position.x < targetTransform.position.x)
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
        }
        
        //���͸� ����
        protected virtual void StopChase()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //������ ���
        private void DropItem(GameObject[] dropItemArray)
        {
            if(dropItemArray == null)
            {
                return;
            }

            foreach (GameObject dorpItem in dropItemArray)
            {
                GameObject item = ObjectPoolManager.Instance.GetPoolObject(dorpItem);
                item.transform.position = transform.position;
            }
        }

        //�������� ������ ����
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        //���
        protected override void Die()
        {
            base.Die();

            GameManager.Instance.OnMonsterDefeated();

            DropItem(coinObject);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //�÷��̾�� �ε������� �÷��̾�� ���ظ� ����
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<PlayerManager>().TakeDamage(bodyAttackDamage);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}