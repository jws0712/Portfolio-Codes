namespace OTO.Object
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Charactor.Monster;
    using OTO.Charactor.Player;
    using OTO.Manager;

    // 총알의 기능을 구현한 클래스
    public class Bullet : MonoBehaviour
    {
        [Header("Bullet")]
        [SerializeField] private float bulletSpeed = default;
        [SerializeField] private float destroyTime = default;
        [SerializeField] private GameObject hitEffect = default;

        //private 변수
        private float bulletDamage = default;

        //프로퍼티
        public float BulletDamage { get{ return bulletDamage; } set{ bulletDamage = value; } }

        void OnEnable()
        {
            StartCoroutine(Co_Destroy(destroyTime));
        }


        private void FixedUpdate()
        {
            transform.Translate(Vector2.right * bulletSpeed * Time.fixedDeltaTime);
        }

        private void SpawnHitEffect()
        {
            if (hitEffect != null)
            {
                GameObject effect = ObjectPoolManager.Instance.GetPoolObject(hitEffect);
                effect.transform.position = transform.position;
            }
        }

        // 총알이 파괴될때 실행
        private void DestroyBullet()
        {
            SpawnHitEffect();
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

        private IEnumerator Co_Destroy(float destroyTime)
        {
            yield return new WaitForSeconds(destroyTime);
            DestroyBullet();

        }
        
        // 총알이 특정 테그와 부딛쳤을때 실행
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageableTarget = collision.GetComponent<IDamageable>();

            if (damageableTarget == null) return;

            damageableTarget.TakeDamage(bulletDamage);

            if(collision.CompareTag("House"))
            {
                return;
            }
            else
            {
                DestroyBullet();
            }
        }
    }
}

