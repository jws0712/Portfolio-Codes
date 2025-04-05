//System
using System;
using System.Collections;

//Unity
using UnityEngine;

//Project
using fademanager;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private GameObject fadeCanvas = null;

    private Animator anim = null;
    private AnimationClip animationClip = null;

    private void Awake()
    {
        #region �̱���
        if (Instance == null)
        {
            Instance = this;
        }
        #endregion

        anim = GetComponentInChildren<Animator>();

        fadeCanvas.gameObject.SetActive(false);
    }

    //���̵� ��
    public void FadeIn(Action function = null)
    {
        fadeCanvas.gameObject.SetActive(true);
        anim.SetTrigger(ClipName.FadeIn);
        StartCoroutine(Co_FadeIn(function));
    }

    private IEnumerator Co_FadeIn(Action function = null)
    {
        yield return new WaitForSeconds(GetClipTime(ClipName.FadeIn));
        if (function != null)
        {
            function();
        }
    }

    //���̵� �ƿ�
    public void FadeOut(Action function = null)
    {
        fadeCanvas.gameObject.SetActive(true);
        anim.SetTrigger(ClipName.FadeOut);
        StartCoroutine(Co_FadeOut(function));
    }

    private IEnumerator Co_FadeOut(Action function = null)
    {
        yield return new WaitForSeconds(GetClipTime(ClipName.FadeOut));
        if (function != null)
        {
            function();
        }

        fadeCanvas.gameObject.SetActive(false);
    }


    //�ִϸ��̼� Ŭ���� �ð��������� �Լ�
    private float GetClipTime(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if(clip.name == clipName)
            {
                animationClip = clip;
                return clip.length;
            }
        }

        Debug.LogError("NOT FOUND CLIPS");
        return 0f;
    }
}
