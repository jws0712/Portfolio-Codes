namespace OTO.Charactor.Monster
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Object;
    using OTO.Manager;
    using OTO.Charactor.Player;

    public class SlimeBoss : Monster
    {
        [Header("BossSlimeJump")]
        [SerializeField] private float jumpPower = default;
        [SerializeField] private LayerMask gorundLayer = default;
        [SerializeField] private Transform groundCheckPos = default;

        [Header("SlimeBossInfo")]
        [SerializeField] private GameObject bulletObject = null;
        [SerializeField] private int bulletNumber = default;
        [SerializeField] private int bulletSpreadAngle = default;

        //private variables
        private Animator animator = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            base.Update();

            animator.SetFloat("YPos", rb.velocity.y);

            if(CheckGround())
            {
                animator.SetBool("isJump", false);
            }
            else
            {
                animator.SetBool("isJump", true);
            }
        }
        
        //���� �Լ�
        protected override void Attack()
        {
            Jump();
        }


        // �Ѿ� ������ ������ �Լ�
        private void FireBullet()
        {
            float startBbulletSpread = bulletSpreadAngle * (bulletNumber / 2);
            AudioManager.Instance.PlaySFX("EarthWormAttack");

            for (int i = 1; i <= bulletNumber; i++)
            {
                Quaternion bulletAngle = Quaternion.Euler(0, 0, startBbulletSpread);

                GameObject bullet = ObjectPoolManager.Instance.GetPoolObject(bulletObject);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = bulletAngle;
                bullet.GetComponent<Bullet>().BulletDamage = attackDamage;

                startBbulletSpread -= bulletSpreadAngle;
            }
        }

        // ������ �����ϴ� �ڵ�
        private void Jump()
        {
            if (CheckGround() == true)
            {
                AudioManager.Instance.PlaySFX("SlimeBossJump");
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                FireBullet();
            }
        }

        // ���� ��Ҵ��� �˻��ϴ� �Լ�
        private bool CheckGround()
        {
            return Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, gorundLayer);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<PlayerManager>().TakeDamage(bodyAttackDamage);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                AudioManager.Instance.PlaySFX("BossLanding");
                FireBullet();
            }
        }
    }
}
