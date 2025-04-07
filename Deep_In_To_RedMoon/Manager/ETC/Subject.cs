namespace OTO.Controller
{
    using System;
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    public class Subject : MonoBehaviour
    {
        public readonly ArrayList observers = new ArrayList();

        //�������� �߰��ϴ� �Լ�
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        //�������� �����ϴ� �Լ�
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        //�������� �̺�Ʈ�� ���� ��Ű�� �Լ�
        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Notify(this);
            }
        }
    }
}


