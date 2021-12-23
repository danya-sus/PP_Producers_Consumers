using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PP_Producers_Consumers
{
    public class CustomQueue<T>
    {
        private readonly int _initialCapacity = 2;
        private T[] _elements;

        private Queue<T> queue = new Queue<T>();

        private Mutex _mutex = new Mutex();

        public CustomQueue()
        {
            this._elements = new T[_initialCapacity];
            this.Count = 0;
        }

        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            _mutex.WaitOne();
            if (this.Count == this._elements.Length)
                Expand();

            queue.Enqueue(item);

            _mutex.ReleaseMutex();
        }
        public T Dequeue()
        {
            _mutex.WaitOne();
            if (queue.Count == 0)
            {
                _mutex.ReleaseMutex();
                return default(T);
            }


            T element = queue.Peek();

            queue.Dequeue();

            _mutex.ReleaseMutex();

            return element;
        }

        private void Expand()
        {
            var newArray = new T[this._elements.Length * 2];
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = _elements[i];
            }
            this._elements = newArray;
        }
    }
}
