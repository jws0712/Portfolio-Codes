namespace OTO.Controller
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    public class CanvasController : MonoBehaviour
    {
        //private variable
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StageEventBus.Subscribe(StageEventType.StartStage, SetPlayerUI);
        }

        // 인게임 UI애니매이션를 실행시키는 함수
        private void SetPlayerUI()
        {
            animator.SetTrigger("SetPlayerUI");
        }
    }
}


