using Xunit;
using CircularList;
using System.Diagnostics;
using System.Collections.Generic;

namespace UnitTests.AddData_Tests
{
    public class Append_Tests
    {
        [Fact]
        public void Append()
        {
            // CircularList
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            CircularList<int> test = new CircularList<int>();
            for (int i = 0; i < 1000000; i++)
            {
                test.Append(i);
            }
            int[] testArray = test.ToArray();
            stopWatch1.Stop();
            // classic method
            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            List<int> test2 = new List<int>();
            for (int i = 0; i < 1000000; i++)
            {
                test2.Add(i);
            }
            int[] testArra2 = test2.ToArray();
            stopWatch2.Stop();
            // TEST EQUALNESS
            Testhelpers.helpers.Equals(testArray, testArra2);
        }
        [Fact]
        public void PerformanceProfile_Append()
        {
            // CircularList
            CircularList<int> test = new CircularList<int>();
            for (int i = 0; i < 10000000; i++)
            {
                test.Append(i);
            }
            //int[] testArray = test.ToArray();
        }
    }
}
