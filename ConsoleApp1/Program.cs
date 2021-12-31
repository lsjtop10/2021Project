using System;
using System.Collections.Generic;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<bool[]> abc = new List<bool[]>();
            bool[] a = { true, true, true, false };
            bool[] b = { false, false, true, true };

            abc.Add(a);
            abc.Add(b);
        }
    }
}
