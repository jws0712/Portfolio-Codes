namespace OTO.Controller
{
    //System
    using System;
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    public class Subject : MonoBehaviour
    {
        public readonly ArrayList observers = new ArrayList();

        //�������� �߰�
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        //�������� ����
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        //�������� �̺�Ʈ�� ����
        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Notify(this);
            }
        }
    }
}


