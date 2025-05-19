namespace OTO.Manager
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Controller;
    using OTO.Charactor.Player;

    public class CameraController : MonoBehaviour, IObserver
    {
        [Header("CameraInfo")]
        [SerializeField] private float followSpeed = default;
        [SerializeField] private float yOffset = default;
        [SerializeField] private float minWorldSize = default;
        [SerializeField] private float maxWorldSize = default;

        //private 변수
        private GameObject followTarget = null;
        private Animator anim = null;

        private PlayerController playerController = null;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            GameManager.Instance.SetController(this);
        }

        private void LateUpdate()
        {
            //카메라가 타겟을 쫒아감
            if (followTarget == null)
            {
                return;
            }
            else
            {
                Vector3 newPos = new Vector3(followTarget.transform.position.x, 0 + yOffset, -10f);
                transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
                float Xpos = transform.position.x;
                Xpos = Mathf.Clamp(transform.position.x, minWorldSize, maxWorldSize);
                transform.position = new Vector3(Xpos, 0 + yOffset, -10f);
            }
        }

        // 카메라 쉐이크 실행
        private void PlayShake(string name)
        {
            if(name == null)
            {
                return;
            }

            anim.SetTrigger(name);
        }


        public void Notify(Subject subject)
        {
            if(!playerController)
            {
                playerController = subject.GetComponent<PlayerController>();
                followTarget = subject.gameObject;
            }
            else
            {
                PlayShake(playerController.CameraShakeType);
                playerController.CameraShakeType = null;
            }
            
        }
    }
}



