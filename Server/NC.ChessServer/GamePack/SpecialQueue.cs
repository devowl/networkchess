using System.Collections;
using System.Collections.Generic;

namespace NC.ChessServer.GamePack
{
    /// <summary>
    /// Removable queue.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class SpecialQueue<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();

        /// <summary>
        /// Items count.
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// Add to queue.
        /// </summary>
        public void Enqueue(T t)
        {
            _list.AddLast(t);
        }

        /// <summary>
        /// Get item.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            var result = _list.First.Value;
            _list.RemoveFirst();
            return result;
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        public bool Remove(T t)
        {
            return _list.Remove(t);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}