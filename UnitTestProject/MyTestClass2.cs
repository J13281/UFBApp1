using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace UnitTestProject
{
    [TestClass]
    public class MyTestClass2
    {
        [TestMethod]
        public void MyTestMethod1()
        {
            var a = m1(new DS4Mock
            {
                btn0 = true,
                btn1 = false,
                btn2 = true,
                btn3 = false,
                l1 = true,
                l2 = false,
                r1 = true,
                r2 = false,
            });

            Console.WriteLine(Convert.ToString(a, 2));
        }

        static int m1(DS4Mock ds4)
        {
            var result = 0b100_00000000000;

            var buttons = new[] {
                ds4.btn0, ds4.btn1, ds4.btn2, ds4.btn3,
                ds4.l1, ds4.l2, ds4.r1, ds4.r2,
            };

            for (var i = 0; i < buttons.Length; i++)
            {
                var b = buttons[i] ? 1 : 0;
                result |= b << i;
            }

            return result;
        }

        static int m2(int n, int m)
        {
            return n | m;
            // 0b000_00000000000
        }
    }
}
