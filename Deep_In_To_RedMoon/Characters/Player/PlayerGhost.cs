namespace OTO.Charactor.Player
{
    using OTO.Manager;

    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    public class PlayerGhost : MonoBehaviour
    {
        [Header("GhostSetting")]
        [SerializeField] private float ghostDelay;
        [SerializeField] private GameObject ghost;
        [SerializeField] private float destoryTime;

        [HideInInspector]
        public bool makeGhost = false;

        //private ����
        private Sprite currentSprite = null;
        private SpriteRenderer spriteRenderer;
        private float ghostDelaySeconds;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ghostDelaySeconds = ghostDelay;
        }

        private void Update()
        {
            //��� �Ҷ� �ܻ��� ��ȯ
            if (makeGhost)
            {
                if (ghostDelaySeconds > 0)
                {
                    ghostDelaySeconds -= Time.deltaTime;
                }
                else
                {
                    GameObject currentGhost = ObjectPoolManager.Instance.GetPoolObject(ghost);
                    currentGhost.transform.position = transform.position;
                    currentGhost.transform.rotation = transform.rotation;
                    currentGhost.transform.localScale = transform.localScale;

                    currentSprite = spriteRenderer.sprite;

                    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                    ghostDelaySeconds = ghostDelay;

                    StartCoroutine(Co_Destroy(currentGhost, destoryTime));
                }
            }
        }

        //�ܻ��� �ı��ϴ� �ڷ�ƾ
        private IEnumerator Co_Destroy(GameObject destroyObject, float destroyTime)
        {
            yield return new WaitForSeconds(destroyTime);
            ObjectPoolManager.Instance.ReturnObject(destroyObject);
        }
    }
}


