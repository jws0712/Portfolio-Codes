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

    // �Ѿ��� ����� ������ Ŭ����
    public class Bullet : MonoBehaviour
    {
        [Header("Bullet")]
        [SerializeField] private float bulletSpeed = default;
        [SerializeField] private float destroyTime = default;
        [SerializeField] private GameObject hitEffect = default;

        //private variable
        private float bulletDamage = default;

        //property
        public float BulletDamage { get{ return bulletDamage; } set{ bulletDamage = value; } }

        void OnEnable()
        {
            StartCoroutine(Co_Destroy(destroyTime));
        }

        /// �Ѿ��� �����̴� �ڵ�
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

        // �Ѿ��� �ı��ɶ� ����Ǵ� �Լ�
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
        
        // �Ѿ��� Ư�� �ױ׿� �ε������� ����Ǵ� �ڵ�
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

