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

        //�̺�Ʈ�� ����ϴ� �Լ�
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

        //�̺�Ʈ ����� �����ϴ� �Լ�
        public static void Unsubscribe(StageEventType type, UnityAction listener)
        {
            UnityEvent thisEvent;

            if(Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        //��ϵǾ� �ִ� �̺�Ʈ�� �����Ű�� �Լ�
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


