using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var bytes = m1(new DS4Mock
            {
                btn0 = true,
                btn1 = false,
                btn2 = true,
                btn3 = false,

                l1 = false,
                l2 = true,
                r1 = false,
                r2 = true,

                left = false,
                up = false,
                right = true,
                down = true,

                lx = 0,
                ly = 0,
                rx = 31,
                ry = 31,
            });

            Assert.AreEqual(bytes[0], 0b000_00101);
            Assert.AreEqual(bytes[1], 0b001_01010);
            Assert.AreEqual(bytes[2], 0b010_01100);

            Assert.AreEqual(bytes[3], 0b011_00000);

            Assert.AreEqual(bytes[4], 0b100_00000);
            Assert.AreEqual(bytes[5], 0b101_00000);
            Assert.AreEqual(bytes[6], 0b110_11111);
            Assert.AreEqual(bytes[7], 0b111_11111);
        }

        static byte[] m1(DS4Mock ds4)
        {
            var d = new[]
            {
                (byte)0b000_00000,
                (byte)0b001_00000,
                (byte)0b010_00000,
                (byte)0b011_00000,
                (byte)0b100_00000,
                (byte)0b101_00000,
                (byte)0b110_00000,
                (byte)0b111_00000,
            };

            var barray = new[] {
                ds4.btn0, ds4.btn1, ds4.btn2, ds4.btn3,
                ds4.l1, ds4.l2, ds4.r1, ds4.r2,
                ds4.left, ds4.up, ds4.right, ds4.down,
            };

            for (int i = 0; i < barray.Length; i++)
            {
                var f = barray[i] ? 1 : 0;
                d[i / 4] |= (byte)(f << i % 4);
            }

            d[4] |= (byte)(ds4.lx);
            d[5] |= (byte)(ds4.ly);
            d[6] |= (byte)(ds4.rx);
            d[7] |= (byte)(ds4.ry);

            return d;
        }
    }
}
