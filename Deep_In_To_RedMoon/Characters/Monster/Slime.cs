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
        [SerializeField] private float groundCheckRange = default;
        [SerializeField] private LayerMask gorundLayer = default;
        [SerializeField] private Transform groundCheckPos = default;


        protected override void OnEnable()
        {
            base.OnEnable();
            currentCoolTime = 0f;
        }

        protected override void Update()
        {
            base.Update();
        }

        //°ø°Ý
        protected override void Attack()
        {
            if (CheckGround())
            {
                AudioManager.Instance.PlaySFX("MiniSlimeJump");
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        //¶¥¿¡ ´Ý¾Ò´ÂÁö °Ë»ç
        private bool CheckGround()
        {
            return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRange, gorundLayer);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag("House") && isAttack)
            {
                collision.gameObject.GetComponent<House>().TakeDamage(bodyAttackDamage);
            }
        }
    }
}


