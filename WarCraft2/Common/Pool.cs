using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCraft2.Common
{
    /// <summary>
    /// Taken from here: http://codecube.net/2010/01/xna-resource-pool/
    /// 'Xna Resource Pool' by Joel Martinez on 1/5/2010
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : class
    {
        private Queue<T> queue;
        private Action<T> newRoutine;
        private Func<T> creator;
        
        public Pool(int capacity = 20, Func<T> creator = null)
        {
            this.queue = new Queue<T>(capacity);
        }
        
        public Pool(Action<T> newRoutine)
            : this()
        {
            this.newRoutine = newRoutine;
        }

        public int Count { get { return this.queue.Count; } }

        private T Create()
        {
            if (creator != null)
                return creator();
            return Activator.CreateInstance<T>();
        }

        public T New()
        {
            T item;

            item = queue.Count > 0 ? queue.Dequeue() : Create();
            if (this.newRoutine != null) this.newRoutine(item);

            return item;
        }

        public void Return(T item)
        {
            queue.Enqueue(item);
        }
    }
}
