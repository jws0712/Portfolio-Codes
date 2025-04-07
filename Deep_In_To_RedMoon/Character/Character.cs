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

    /// <summary>
    /// ������ �ִ� ��� ��ü���� �θ�Ŭ����
    /// </summary>
    public class Character : MonoBehaviour, IDamageable
    {
        [Header("Material")]
        [SerializeField] protected Material flashMaterial = null;

        [Header("CharactorInfo")]
        [SerializeField] protected float maxHp = default;

        protected float currentHp = default;
        protected new SpriteRenderer renderer = null;
        protected Material originMaterial = null;

        protected bool isDead = default;

        private const float duration = 0.05f;

        /// <summary>
        /// ���� �ʱ�ȭ
        /// </summary>
        protected virtual void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            originMaterial = renderer.material;

            currentHp = maxHp;
        }

        /// <summary>
        /// ������� ������ ����Ǵ� �Լ�
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(float damage)
        {
            currentHp -= damage;

            SpriteFlash();

            if ((!isDead && currentHp <= 0))
            {
                Die();
            }
        }

        //��� ó��
        protected virtual void Die() 
        {
            isDead = true;
            AudioManager.Instance.PlaySFX("Dead");
        }

        /// <summary>
        /// ������� �޾����� �����Ÿ� ����� ������ �Լ�
        /// </summary>
        private void SpriteFlash()
        {
            StartCoroutine(Co_SpriteFlash());
        }


        /// <summary>
        /// ��������Ʈ�� �����Ÿ��� ������ �ڷ�ƾ
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_SpriteFlash()
        {
            renderer.material = flashMaterial;

            yield return new WaitForSeconds(duration);

            renderer.material = originMaterial;
        }
    }
}