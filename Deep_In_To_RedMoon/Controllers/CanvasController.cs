namespace OTO.Controller
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    public class CanvasController : MonoBehaviour
    {
        //private ����
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StageEventBus.Subscribe(StageEventType.StartStage, SetPlayerUI);
        }

        // �ΰ��� UI�ִϸ��̼Ǹ� ����
        private void SetPlayerUI()
        {
            animator.SetTrigger("SetPlayerUI");
        }
    }
}


