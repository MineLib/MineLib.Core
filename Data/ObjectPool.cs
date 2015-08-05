using System;
using System.Collections;
using System.Collections.Generic;

namespace MineLib.Core.Data
{
    // From TechCraft

    /// <summary>
    /// Represents a fixed-size pool of available items that can be removed
    /// as needed and returned when finished.
    /// </summary>
    public class Pool<T> : IEnumerable<T> where T : new()
    {
        /// <summary>
        /// Represents an entry in a Pool collection.
        /// </summary>
        public struct Node
        {
            /// <summary>
            /// Used internally to track which entry in the Pool
            /// is associated with this Node.
            /// </summary>
            internal int NodeIndex;

            /// <summary>
            /// Item stored in Pool.
            /// </summary>
            public T Item;
        }


        /// <summary>
        /// Fixed Pool of item nodes.
        /// </summary>
        private readonly Node[] _pool;


        /// <summary>
        /// Array containing the active/available state for each item node 
        /// in the Pool.
        /// </summary>
        private readonly bool[] _active;


        /// <summary>
        /// Queue of available item node indices.
        /// </summary>
        private readonly Queue<int> _available;


        /// <summary>
        /// Gets the number of available items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int AvailableCount { get { return _available.Count; } }


        /// <summary>
        /// Gets the number of active items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int ActiveCount { get { return _pool.Length - _available.Count; } }


        /// <summary>
        /// Gets the total number of items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int Capacity { get { return _pool.Length; } }


        /// <summary>
        /// Initializes a new instance of the Pool class.
        /// </summary>
        /// <param name="numItems">Total number of items in the Pool.</param>
        /// <exception cref="ArgumentException">
        /// Number of items is less than 1.
        /// </exception>
        /// <remarks>
        /// This constructor is an O(n) operation, where n is capacity.
        /// </remarks>
        public Pool(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("capacity", "Pool must contain at least one item.");
            
            _pool = new Node[capacity];
            _active = new bool[capacity];
            _available = new Queue<int>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                _pool[i] = new Node();
                _pool[i].NodeIndex = i;
                _pool[i].Item = new T();

                _active[i] = false;
                _available.Enqueue(i);
            }
        }


        /// <summary>
        /// Makes all items in the Pool available.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, where n is Capacity.
        /// </remarks>
        public void Clear()
        {
            _available.Clear();

            for (int i = 0; i < _pool.Length; i++)
            {
                _active[i] = false;
                _available.Enqueue(i);
            }
        }


        /// <summary>
        /// Removes an available item from the Pool and makes it active.
        /// </summary>
        /// <returns>The node that is removed from the available Pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no available items in the Pool.
        /// </exception>
        /// <remarks>
        /// This method is an O(1) operation.
        /// </remarks>
        public Node Get()
        {
            int nodeIndex = _available.Dequeue();
            _active[nodeIndex] = true;
            return _pool[nodeIndex];
        }


        /// <summary>
        /// Returns an active item to the available Pool.
        /// </summary>
        /// <param name="item">The node to return to the available Pool.</param>
        /// <exception cref="ArgumentException">
        /// The node being returned is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The node being returned was not active.
        /// This probably means the node was previously returned.
        /// </exception>
        /// <remarks>
        /// This method is an O(1) operation.
        /// </remarks>
        public void Return(Node item)
        {
            if ((item.NodeIndex < 0) || (item.NodeIndex > _pool.Length))
                throw new ArgumentException("Invalid item node.");
            
            if (!_active[item.NodeIndex])
                throw new InvalidOperationException("Attempt to return an inactive node.");       

            _active[item.NodeIndex] = false;
            _available.Enqueue(item.NodeIndex);
        }


        /// <summary>
        /// Sets the value of the item in the Pool associated with the 
        /// given node.
        /// </summary>
        /// <param name="item">The node whose item value is to be set.</param>
        /// <exception cref="ArgumentException">
        /// The node being returned is invalid.
        /// </exception>
        /// <remarks>
        /// This method is necessary to modify the value of a value type stored
        /// in the Pool.  It copies the value of the node's Item field into the
        /// Pool.
        /// This method is an O(1) operation.
        /// </remarks>
        public void SetItemValue(Node item)
        {
            if ((item.NodeIndex < 0) || (item.NodeIndex > _pool.Length))
                throw new ArgumentException("Invalid item node.");
            
            _pool[item.NodeIndex].Item = item.Item;
        }


        /// <summary>
        /// Copies the active items to an existing one-dimensional Array, 
        /// starting at the specified array index. 
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array to which active Pool items will be 
        /// copied.
        /// </param>
        /// <param name="arrayIndex">
        /// The index in array at which copying begins.
        /// </param>
        /// <returns>The number of items copied.</returns>
        /// <remarks>
        /// This method is an O(n) operation, where n is the smaller of 
        /// capacity or the array length.
        /// </remarks>
        public int CopyTo(T[] array, int arrayIndex)
        {
            int index = arrayIndex;

            foreach (Node item in _pool)
                if (_active[item.NodeIndex])
                {
                    array[index++] = item.Item;

                    if (index == array.Length)
                        return index - arrayIndex;
                }

            return index - arrayIndex;
        }


        /// <summary>
        /// Gets an enumerator that iterates through the active items 
        /// in the Pool.
        /// </summary>
        /// <returns>Enumerator for the active items.</returns>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (Node item in _pool)
                if (_active[item.NodeIndex])
                    yield return item.Item;
        }


        /// <summary>
        /// Gets an enumerator that iterates through the active nodes 
        /// in the Pool.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// </remarks>
        public IEnumerable<Node> ActiveNodes
        {
            get
            {
                foreach (Node item in _pool)
                    if (_active[item.NodeIndex])
                        yield return item;
            }
        }


        /// <summary>
        /// Gets an enumerator that iterates through all of the nodes 
        /// in the Pool.
        /// </summary>
        /// <remarks>
        /// This method is an O(1) operation. 
        /// </remarks>
        public IEnumerable<Node> AllNodes
        {
            get
            {
                foreach (Node item in _pool)
                    yield return item;
            }
        }


        /// <summary>
        /// Implementation of the IEnumerable interface.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
