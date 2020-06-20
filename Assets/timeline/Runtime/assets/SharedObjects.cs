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
                return new T();
            }
            var t = listPool.First.Value;
            listPool.RemoveFirst();
            return t;
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
