using System.Collections.Generic;
using System.Linq;
using System;

namespace Bookkeeping_manager
{
    public class IndexSet<T>
    {
        private readonly List<T> arr;
        private readonly HashSet<T> set;
        /// <summary>
        /// Returns the object at the given index in arr
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => arr[index];
            set => arr[index] = value;
        }
        /// <summary>
        /// returns the object that matches the hase code of item from arr
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public T this[T item]
        {
            get => arr.Find(x => x.GetHashCode() == item.GetHashCode());
            set
            {
                int v = arr.FindIndex(x => x.GetHashCode() == item.GetHashCode());
                arr[v] = item;
            }
        }
        public int Length => arr.Count;
        public static explicit operator IndexSet<T>(List<T> b) => new IndexSet<T>(b);
        /// <summary>
        /// initalises arr and set to new objects
        /// </summary>
        public IndexSet()
        {
            arr = new List<T>();
            set = new HashSet<T>();
        }
        /// <summary>
        /// adds all items in a to the arr and set
        /// </summary>
        /// <param name="a"></param>
        public IndexSet(List<T> a) : this()
        {
            foreach (T b in a)
            {
                Add(b);
            }
        }
        /// <summary>
        /// true if added cant add if hash code exists in it
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Add(T data)
        {
            if (set.Add(data))
            {
                arr.Add(data);
                return true;
            }
            return false;
        }
        /// <summary>
        /// compares the has codes present in set
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Contains(T a)
        {
            return set.Contains(a);
        }
        /// <summary>
        /// returns and removes the last element
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T d = arr.Last();
            set.Remove(d);
            arr.Remove(d);
            return d;
        }
        /// <summary>
        /// returns and removes the element at the gieven index
        /// </summary>
        /// <returns></returns>
        public T Pop(int index)
        {
            T d = arr[index];
            set.Remove(d);
            arr.RemoveAt(index);
            return d;
        }
        /// <summary>
        /// returns and removes the element with a mathicng hash code
        /// </summary>
        /// <returns></returns>
        public T Pop(T d)
        {
            set.Remove(d);
            arr.Remove(d);
            return d;
        }
        /// <summary>
        /// removes at the given index
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            T found = arr[index];
            Remove(found);
        }
        /// <summary>
        /// removes based on matching hash code
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            try
            {
                arr.Remove(item);
                set.Remove(item);
            }
            catch
            {

            }
        }
        /// <summary>
        /// removes where condition is met only once
        /// </summary>
        public void Remove(Predicate<T> condition)
        {
            T found = arr.Find(condition);
            Remove(found);
        }
    }
}
