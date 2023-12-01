using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2023
{
    public static class Input
    {
        public static string Get()
        {
            return File.OpenText(@"..\..\..\Input.txt").ReadToEnd();
        }
    }
}