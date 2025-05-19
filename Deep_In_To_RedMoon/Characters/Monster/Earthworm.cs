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

    public class Earthworm : Monster
    {
        [Header("BulletInfo")]
        [SerializeField] private GameObject bulletObject = null;
        [SerializeField] private Transform firePos = null;
        [SerializeField] private int bulletNumber = default;
        [SerializeField] private int bulletSpreadAngle = default;

        private float rotZ = default;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Update()
        {

            base.Update();

            if (isChasePlayer)
            {
                GetFireRot(playerTransform);
            }
            else
            {
                GetFireRot(houseTransform);
            }
        }

        //공격
        protected override void Attack()
        {
            float startBbulletSpread = rotZ + (bulletSpreadAngle * (bulletNumber / 2));
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

        //타겟의 위치를 받아와서 총알의 발사 각도를 계산
        private void GetFireRot(Transform targetPos)
        {
            Vector3 rotation = targetPos.position - firePos.position;

            rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        }

    }

}

