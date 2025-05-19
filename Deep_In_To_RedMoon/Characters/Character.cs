namespace OTO.Charactor
{
    //System
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.Experimental.GlobalIllumination;

    //Project
    using OTO.Manager;

    public class Character : MonoBehaviour, IDamageable
    {
        [Header("Material")]
        [SerializeField] protected Material flashMaterial = null;

        [Header("CharactorInfo")]
        [SerializeField] protected float maxHp = default;
        [SerializeField] protected float spriteFlashDuration = 0.05f;

        protected float currentHp = default;
        protected new SpriteRenderer renderer = null;
        protected Material originMaterial = null;
        protected bool isDead = default;

        //�ʱ�ȭ
        protected virtual void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            originMaterial = renderer.material;

            currentHp = maxHp;
        }

        // ������� ������ ����
        public virtual void TakeDamage(float damage)
        {
            currentHp -= damage;

            SpriteFlash();

            if ((!isDead && currentHp <= 0))
            {
                Die();
            }
        }

        //���
        protected virtual void Die() 
        {
            isDead = true;
            AudioManager.Instance.PlaySFX("Dead");
        }

        //��������Ʈ�� �����Ÿ��� ��
        private void SpriteFlash()
        {
            StartCoroutine(Co_SpriteFlash());
        }

        private IEnumerator Co_SpriteFlash()
        {
            renderer.material = flashMaterial;

            yield return new WaitForSeconds(spriteFlashDuration);

            renderer.material = originMaterial;
        }
    }
}