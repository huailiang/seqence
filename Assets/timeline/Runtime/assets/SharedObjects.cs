using System.Collections.Generic;

namespace UnityEngine.Timeline
{
    public interface ISharedObject
    {
        void Dispose();
    }

    public class SharedObjects<T> where T : ISharedObject, new()
    {
        private static LinkedList<T> listPool = new LinkedList<T>();

        public static T Get()
        {
            if (listPool.Count <= 0)
            {
                T t = new T();
                listPool.AddLast(t);
                return t;
            }
            var t2 = listPool.First.Value;
            listPool.RemoveFirst();
            return t2;
        }

        public static void Return(T obj)
        {
            obj.Dispose();
            listPool.AddLast(obj);
        }

        public static void Clean()
        {
            foreach (var it in listPool)
            {
                it.Dispose();
            }
            listPool.Clear();
        }
    }
}
