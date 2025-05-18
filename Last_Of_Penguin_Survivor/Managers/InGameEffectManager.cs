namespace Lop.Survivor.Island.Effect
{
    using JetBrains.Annotations;
    // # System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // # UnityEngine
    using UnityEngine;
    using UnityEngine.Pool;

    // # Mirror
    using Mirror;

    public class InGameEffectManager : NetworkBehaviour
    {
        public static InGameEffectManager Instance;

        [Header(" # EffectManager_Info")]
        [SerializeField] private EffectData[] effectArray = null;
        [SerializeField] private float poolMaxSize =        default;

        private Dictionary<string, IObjectPool<Effect>> effectPools;

        //싱글톤
        private void Awake()
        {
            #region 싱글톤
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(Instance.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
            #endregion

            effectPools = new Dictionary<string, IObjectPool<Effect>>();

            // 각 이펙트에 대한 풀 생성
            foreach (var effectData in effectArray)
            {
                effectPools[effectData.effectName] = new ObjectPool<Effect>(
                    () => CreateEffect(effectData.effectPrefab),
                    OnGetEffect,
                    OnReleaseEffect,
                    OnDestroyEffect,
                    maxSize: (int)poolMaxSize
                );
            }
        }

        //이펙트 재생
        public void PlayEffect(string effectName, string sfxName, Vector3 position)
        {
            if(effectPools.TryGetValue(effectName, out IObjectPool<Effect> pool))
            {
                SoundManager.Instance.PlayMusic(sfxName);
                Effect effect = pool.Get();
                effect.transform.position = position;
                effect.transform.rotation = Quaternion.identity;

                SendEffectMessage(effectName, sfxName, position);
            }
            else
            {
                Debug.LogError("Not Found Effect.");
            }
        }

        //이펙트 생성
        private Effect CreateEffect(GameObject effectPrefab)
        {
            Effect effect = Instantiate(effectPrefab).GetComponent<Effect>();
            effect.SetManagedPool(effectPools[effectPrefab.name]);
            return effect;
        }

        //이펙트를 가져올때 실행
        private void OnGetEffect(Effect effect)
        {
            effect.gameObject.SetActive(true);
        }

        //이펙트의 사용이 끝났을때 실행
        private void OnReleaseEffect(Effect effect)
        {
            effect.gameObject.SetActive(false);
        }

        //이펙트를 파괴할때 실행
        private void OnDestroyEffect(Effect effect)
        {
            Destroy(effect.gameObject);
        }
    }
}
