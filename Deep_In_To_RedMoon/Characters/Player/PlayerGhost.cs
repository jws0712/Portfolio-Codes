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

        //private 변수
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
            //대시 할때 잔상을 소환
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

        //잔상을 파괴하는 코루틴
        private IEnumerator Co_Destroy(GameObject destroyObject, float destroyTime)
        {
            yield return new WaitForSeconds(destroyTime);
            ObjectPoolManager.Instance.ReturnObject(destroyObject);
        }
    }
}


