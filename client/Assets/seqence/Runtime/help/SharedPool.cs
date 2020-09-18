using System;

namespace UnityEngine.Seqence
{

    // next 加在这里，主要是为了避免像LinkList一样, 每个节点都要new一个LinkedListNode
    public interface ISharedObject<T> where T : class, new()
    {
        T next { get; set; }

        void Dispose();
    }


    // 基于单向链表实现的队列，便于插入和删除，不适合查找， 比较适合缓冲池
    public sealed class LinkQueue<T> where T : class, ISharedObject<T>, new()
    {
        private T _head, _tail;
        private int cnt = 0;

        public int Count
        {
            get { return cnt; }
        }

        public void Enqueue(T it)
        {
            cnt++;
            if (_head == null)
            {
                _head = it;
                _tail = _head;
            }
            else
            {
                _tail.next = it;
            }
            _tail = it;
        }

        public T Dequeue()
        {
            if (cnt <= 0)
            {
                throw new Exception("link queue is null");
            }
            var v = _head;
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

        public void For(Action<ISharedObject<T>> cb)
        {
            var p = _head;
            while (p != null)
            {
                cb(p);
                p = p.next as T;
            }
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            cnt = 0;
        }
    }

    public class SharedPool<T> where T : class, ISharedObject<T>, new()
    {
        private static LinkQueue<T> pool = new LinkQueue<T>();

        public static T Get()
        {
            if (pool.Count <= 0)
            {
                return new T();
            }
            return pool.Dequeue();
        }

        public static void Return(T obj)
        {
            obj.Dispose();
            pool.Enqueue(obj);
        }

        public static void Clean()
        {
            pool.For(it => it.Dispose());
            pool.Clear();
        }

    }

}