namespace OTO.Manager
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEnigne
    using UnityEngine;

    //�̱���
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        //���� ���� ����
        private static T instance;

        //�ٸ� ������ ����� �� �ִ� �ν��Ͻ�
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>(); //������Ʈ Ÿ���� ã�� �Ҵ���
                    if (instance == null) //������Ʈ Ÿ���� ��ã����
                    {
                        GameObject obj = new GameObject(); //������Ʈ�� ���� ����
                        obj.name = typeof(T).Name; //������Ʈ�� �̸��� Ŭ���� �̸����� ����
                        instance = obj.AddComponent<T>(); //������Ʈ�� Ŭ������ �߰�
                    }
                }

                return instance; //�ν��Ͻ��� ��ȯ
            }
        }

        //��� �޾Ƽ� ������ �� �� �ִ�
        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T; //Ŭ������ ��ȯ
                DontDestroyOnLoad(gameObject); //���α׷��� �����ϱ� ������ ��� ��� �ְ� ��
            }
            else
            {
                Destroy(gameObject); //�̹� ���� �ν��Ͻ��� �ִٸ� �ڽ��� ������
            }
        }
    }
}



