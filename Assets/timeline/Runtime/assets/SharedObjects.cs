using System.Collections.Generic;

namespace UnityEngine.Timeline
{
    public interface ISharedObject
    {
        void Dispose();
    }

    public class SharedObjects<T> where T : ISharedObject, new()
    {
        private static Queue<T> listPool = new Queue<T>();

        public static T Get()
        {
            if (listPool.Count <= 0)
            {
                return new T();
            }
            return listPool.Dequeue();
        }

        public static void Return(T obj)
        {
            obj.Dispose();
            listPool.Enqueue(obj);
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
