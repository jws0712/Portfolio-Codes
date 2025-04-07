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

            //�̰� ���ľ���
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

        //���� Ŭ������ �ʵ带 �ʱ�ȭ �ϴ� �Լ�
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

        //���� ��Ÿ���� �� ���� ������ �����ϴ� �Լ�
        protected virtual void Attack()
        {
            currentCoolTime += Time.deltaTime;

            if (currentCoolTime >= attackCoolTime)
            {
                isAttack = true;
            }
        }

        //���� �÷��̾ ���ݰŸ��� �ִ��� �˻��ϴ� �Լ�
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

        //�����Ÿ� ���� �÷��̾ �ִ��� �Ǵ��ϴ� �Լ�
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

        //Ÿ������ �����Ÿ� �ٰ����� ���ߴ� �Լ�
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

        //Ÿ���� �����ϴ� �Լ�
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
        
        //���͸� ���ߴ� �Լ�
        protected virtual void StopChase()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //���Ͱ� �׾����� �������� ����߸��� �Լ�
        private void DropItem(params GameObject[] dropItem)
        {
            for(int i = 0; i < dropItem.Length; i++)
            {
                GameObject item = ObjectPoolManager.Instance.GetPoolObject(dropItem[i]);
                item.transform.position = transform.position;

                item.GetComponent<Rigidbody2D>().AddForce(Vector2.one * -2f, ForceMode2D.Impulse);
            }
        }

        //�������� ������ ����Ǵ� �Լ�
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        //���Ͱ� �׾����� ����Ǵ� �Լ�
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

        //�÷��̾�� �ε������� �÷��̾�� ���ظ� ����
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