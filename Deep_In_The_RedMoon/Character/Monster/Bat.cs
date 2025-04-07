namespace OTO.Charactor.Monster
{

    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Object;

    public class Bat : Monster
    {
        [Header("BatInfo")]
        [SerializeField] private GameObject bulletObject = null;
        [SerializeField] private int bulletNumber = default;
        [SerializeField] private int bulletSpeadAngle = default;
        [SerializeField] private int startBulletSpreadAngle = default;

        protected override void OnEnable()
        {
            chaseHouse = true;

            base.OnEnable();
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
                float bulletSpread = transform.rotation.z + startBulletSpreadAngle;
                for (int i = 0; i < bulletNumber; i++)
                {
                    Quaternion bulletAngle = Quaternion.Euler(0, 0, bulletSpread);
                    GameObject _bullet = Instantiate(bulletObject, transform.position, bulletAngle);
                    _bullet.GetComponent<Bullet>().BulletDamage = attackDamage;
                    bulletSpread -= bulletSpeadAngle;

                }
                bulletSpread = transform.rotation.z + startBulletSpreadAngle * 2;

                isAttack = false;
                currentCoolTime = 0;
            }
        }
    }
}


