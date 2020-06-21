using System;

namespace UnityEngine.Timeline
{
    public interface ISharedObject
    {
        void Dispose();
    }


    public class LinkNode<T> where T : new()
    {
        public LinkNode<T> next;
        public T Value { get; }

        public LinkNode(T t)
        {
            this.Value = t;
        }
    }


    // 基于单向链表实现的队列，便于插入和删除，不适合查找
    public sealed class LinkQueue<T> where T : new()
    {
        private LinkNode<T> _head, _tail;
        private int cnt = 0;

        public int Count
        {
            get { return cnt; }
        }

        public void Enqueue(T it)
        {
            cnt++;
            var n = new LinkNode<T>(it);
            if (_head == null)
            {
                _head = n;
                _tail = _head;
            }
            else
            {
                _tail.next = n;
            }
            _tail = n;
        }

        public T Dequeue()
        {
            if (cnt <= 0)
            {
                throw new Exception("link queue is null");
            }
            var v = _head.Value;
            if (_head.next != null)
            {
                _head = _head.next;
            }
            else
            {
                _tail = null;
                _head = null;
            }
            cnt--;
            return v;
        }

        public void For(Action<T> cb)
        {
            var p = _head;
            while (p != null)
            {
                cb(p.Value);
                p = p.next;
            }
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            cnt = 0;
        }
    }

    public class SharedObjects<T> where T : ISharedObject, new()
    {
        private static LinkQueue<T> queue = new LinkQueue<T>();

        public static T Get()
        {
            if (queue.Count <= 0)
            {
                return new T();
            }
            return queue.Dequeue();
        }

        public static void Return(T obj)
        {
            obj.Dispose();
            queue.Enqueue(obj);
        }

        public static void Clean()
        {
            queue.For(it => it.Dispose());
            queue.Clear();
        }
    }
}
