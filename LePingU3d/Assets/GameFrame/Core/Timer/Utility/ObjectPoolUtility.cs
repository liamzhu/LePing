using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class ObjectPoolUtility<T> where T : new() {
        private readonly List<T> mUsed = new List<T>();
        private readonly List<T> mUnused = new List<T>();

        public List<T> usingObjects {
            get { return mUsed; }
        }

        //public List<T> unusingObjects {
        //    get { return mUnused; }
        //}


        public T Spawn()
        {
            if (mUnused.Count > 0) {
                T item = mUnused[0];
                mUsed.Add(item);
                mUnused.RemoveAt(0);
                return item;
            }
            else {
                T item = new T();
                mUsed.Add(item);
                return item;
            }
        }

        public bool Recycle(T item)
        {
            if (mUsed.Remove(item))
            {
                
            }
            mUnused.Add(item);
            return true;
        }
    }

}
