using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nimble_life
{
    public static class Rando
    {
        private static Random r = new Random(Guid.NewGuid().GetHashCode());
        public static int Next(int limit)
        {
            return r.Next(limit);
        }

        // A number between 0 and 1.
        public static double Next()
        {
            return r.NextDouble();
        }

        public static float Either(float num1, float num2)
        {
            return r.Next(1) == 0 ? num1 : num2;
        }

        // shuffle the members of a list (in-place)
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                //swap n with k.
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
