using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterWF
{
    static class Extensions
    {
        public static IObservable<T> Clone<T>(this IObservable<T> listToClone) where T : ICloneable
        {
            return listToClone.Clone();
        }
    }
}
