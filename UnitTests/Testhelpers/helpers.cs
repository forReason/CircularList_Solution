using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Testhelpers
{
    internal class helpers
    {
        internal void CompareArrays<Type> (Type[] array1, Type[] array2)
        {
            if (array1.Length != array2.Length) throw new ArgumentOutOfRangeException("the two array lengths do not match!");
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].ToString() != array2[i].ToString()) throw new DataMisalignedException("both elements are not equal!");
            }

        }
    }
}
