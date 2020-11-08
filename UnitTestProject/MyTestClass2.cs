using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices.ComTypes;

namespace UnitTestProject
{
    [TestClass]
    public class MyTestClass2
    {
        [TestMethod]
        public void MyTestMethod1()
        {
            m1(0b000_00101);
            m1(0b001_01010);
            m1(0b010_01100);
            m1(0b011_00000);
            m1(0b100_00000);
            m1(0b101_00000);
            m1(0b110_11111);
            m1(0b111_11111);
        }

        static void m1(byte b)
        {

        }
    }
}
