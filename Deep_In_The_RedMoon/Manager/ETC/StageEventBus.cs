namespace OTO.Controller
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.Events;

    public class StageEventBus
    {
        private static readonly IDictionary<StageEventType, UnityEvent> Events = new Dictionary<StageEventType, UnityEvent>();

        //이벤트를 등록하는 함수
        public static void Subscribe(StageEventType eventType, UnityAction listener)
        {
            UnityEvent thisEvent;

            if(Events.TryGetValue(eventType, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventType, thisEvent);
            }
        }

        //이벤트 등록을 해제하는 함수
        public static void Unsubscribe(StageEventType type, UnityAction listener)
        {
            UnityEvent thisEvent;

            if(Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        //등록되어 있는 이벤트를 실행시키는 함수
        public static void Publish(StageEventType type)
        {
            UnityEvent thisEvent;

            if(Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}


