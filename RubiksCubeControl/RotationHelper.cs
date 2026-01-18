using System;
using System.Collections.Generic;
using System.Linq;

namespace RubiksCubeControl
{
    public static class RotationHelper
    {
        /// <summary>
        /// Pops three items off the end of a collection, and pushes them onto the start. 
        /// </summary>        
        public static void Rotate<T>(List<T> collection) where T : class
        {
            int index = collection.Count - 1;

            T tmp1 = collection[index - 2];
            T tmp2 = collection[index - 1];
            T tmp3 = collection[index];

            collection.Remove(tmp1); collection.Remove(tmp2); collection.Remove(tmp3);

            collection.Insert(0, tmp3);
            collection.Insert(0, tmp2);
            collection.Insert(0, tmp1);
        }

        /// <summary>
        /// Pops the first three items off the beginning of a collection, and pushes them onto the end. 
        /// </summary>        
        public static void CounterRotate<T>(List<T> collection) where T : class
        {
            int index = 0;

            T tmp1 = collection[index];
            T tmp2 = collection[index + 1];
            T tmp3 = collection[index + 2];

            collection.Remove(tmp1); collection.Remove(tmp2); collection.Remove(tmp3);

            collection.Add(tmp1);
            collection.Add(tmp2);
            collection.Add(tmp3);
        }
    }
}
