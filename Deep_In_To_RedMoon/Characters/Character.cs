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

        //초기화
        protected virtual void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            originMaterial = renderer.material;

            currentHp = maxHp;
        }

        // 대미지를 받을때 실행
        public virtual void TakeDamage(float damage)
        {
            currentHp -= damage;

            SpriteFlash();

            if ((!isDead && currentHp <= 0))
            {
                Die();
            }
        }

        //사망
        protected virtual void Die() 
        {
            isDead = true;
            AudioManager.Instance.PlaySFX("Dead");
        }

        //스프라이트를 깜빡거리게 함
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