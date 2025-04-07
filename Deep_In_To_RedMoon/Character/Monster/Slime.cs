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

    public class Slime : Monster
    {
        [Header("SlimeJump")]
        [SerializeField] private float jumpPower = default;
        [SerializeField] private LayerMask gorundLayer = default;
        [SerializeField] private Transform groundCheckPos = default;

        //private variables
        private bool isHouseAttack = default;

        protected override void OnEnable()
        {
            base.OnEnable();
            chaseHouse = true;
            currentCoolTime = 0f;
        }

        protected override void Update()
        {
            base.Update();
        }

        //공격 함수
        protected override void Attack()
        {
            base.Attack();
            if (isAttack == true)
            {

                if (CheckGround() == true)
                {
                    AudioManager.Instance.PlaySFX("MiniSlimeJump");
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    isHouseAttack = true;
                }
                isAttack = false;
                currentCoolTime = 0;
            }
        }

        //땅에 닫았는지 검사하는 함수
        private bool CheckGround()
        {
            return Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, gorundLayer);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag("House") && isHouseAttack == true)
            {
                collision.gameObject.GetComponent<House>().TakeDamage(bodyAttackDamage);
                isHouseAttack = false;
            }
        }
    }
}


