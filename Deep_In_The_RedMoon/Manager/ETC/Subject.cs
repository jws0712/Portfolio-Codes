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

        //옵저버를 추가하는 함수
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        //옵저버를 삭제하는 함수
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        //옵저버의 이벤트를 실행 시키는 함수
        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Notify(this);
            }
        }
    }
}


