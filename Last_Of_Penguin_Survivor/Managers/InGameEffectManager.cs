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

        //�̱���
        private void Awake()
        {
            #region �̱���
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

            // �� ����Ʈ�� ���� Ǯ ����
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

        //����Ʈ ���
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

        //����Ʈ ����
        private Effect CreateEffect(GameObject effectPrefab)
        {
            Effect effect = Instantiate(effectPrefab).GetComponent<Effect>();
            effect.SetManagedPool(effectPools[effectPrefab.name]);
            return effect;
        }

        //����Ʈ�� �����ö� ����
        private void OnGetEffect(Effect effect)
        {
            effect.gameObject.SetActive(true);
        }

        //����Ʈ�� ����� �������� ����
        private void OnReleaseEffect(Effect effect)
        {
            effect.gameObject.SetActive(false);
        }

        //����Ʈ�� �ı��Ҷ� ����
        private void OnDestroyEffect(Effect effect)
        {
            Destroy(effect.gameObject);
        }
    }
}
