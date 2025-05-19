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

        //옵저버를 추가
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        //옵저버를 삭제
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        //옵저버의 이벤트를 실행
        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Notify(this);
            }
        }
    }
}


