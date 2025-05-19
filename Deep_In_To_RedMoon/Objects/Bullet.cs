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

        //private ����
        private float bulletDamage = default;

        //������Ƽ
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

        // �Ѿ��� �ı��ɶ� ����
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
        
        // �Ѿ��� Ư�� �ױ׿� �ε������� ����
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

