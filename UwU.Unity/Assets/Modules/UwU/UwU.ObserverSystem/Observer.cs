using System.Collections.Generic;
using UwU.TypeId;
using UwU.DI;

namespace UwU.ObserverSystem
{
    public class Observer
    {
        [Inject] private readonly IdProvider idProvider;
        private readonly Dictionary<int, List<ISubscriber>> subscribers;

        public Observer()
        {
            this.subscribers = new Dictionary<int, List<ISubscriber>>(16);
        }

        public void Subscribe<Arg>(ISubscriber subscriber)
        {
            var index = this.idProvider.GetId<Arg>();

            if (this.subscribers.ContainsKey(index))
            {
                this.subscribers[index].Add(subscriber);
            }
            else
            {
                this.subscribers.Add(index, new List<ISubscriber>() { subscriber });
            }
        }

        public void Unsubscribe<Arg>(ISubscriber subscriber)
        {
            var index = this.idProvider.GetId<Arg>();

            if (this.subscribers.ContainsKey(index))
            {
                this.subscribers[index].Remove(subscriber);
            }
        }

        public void Notify<Arg>(Arg arg) where Arg : struct
        {
            var index = this.idProvider.GetId<Arg>();

            if (this.subscribers.ContainsKey(index))
            {
                var notifyTargets = this.subscribers[index];
                for (var i = 0; i < notifyTargets.Count; i++)
                {
                    (notifyTargets[i] as IReactOn<Arg>).OnNotify(arg);
                }
            }
        }

        public void Notify<Arg>() where Arg : struct
        {
            Notify<Arg>(default);
        }
    }
}