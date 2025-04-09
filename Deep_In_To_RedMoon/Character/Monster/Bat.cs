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

    public class Bat : Monster
    {
        [Header("BatInfo")]
        [SerializeField] private GameObject bulletObject = null;
        [SerializeField] private int bulletNumber = default;
        [SerializeField] private int bulletSpreadAngle = default;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Update()
        {
            base.Update();
        }
        
        //공격 함수
        protected override void Attack()
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
    }
}


