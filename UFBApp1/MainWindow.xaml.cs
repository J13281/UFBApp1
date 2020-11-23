using Pad1;
using SharpDX.DirectInput;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Media;

namespace UFBApp1
{
    public partial class MainWindow : Window
    {
        WrapedDS4 ds4;
        SerialPort serial;

        double next = Environment.TickCount;
        double wait = 1000.0 / 60.0;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Environment.TickCount < next)
            {
                return;
            }
            else
            {
                next += wait;
                Loop();
            }
        }

        void Init()
        {
            ds4 = WrapedDS4.initFirst(new InputRange(0, 0b11111111111));
            serial = new SerialPort { BaudRate = 9600, PortName = "COM3" };
            serial.Open();
        }

        void Loop()
        {
            ds4.Update();
            var bytes = Logics.parse(ds4);
            serial.Write(bytes, 0, bytes.Length);
        }
    }

    static class Logics
    {
        static int buttons(int result, bool[] buttons)
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var b = buttons[i] ? 0 : 1;
                result |= b << i;
            }

            return result;
        }

        static void divide(int n, out byte high, out byte low)
        {
            high = (byte)(1 << 7 | n >> 7);
            low = (byte)(n & 0b1111111);
        }

        public static byte[] parse(WrapedDS4 ds4)
        {
            var e = buttons(
                0b100_00000000000, new[] {
                    ds4.btn0, ds4.btn1, ds4.btn2, ds4.btn3,
                    ds4.l1, ds4.l2, ds4.r1, ds4.r2, });

            var a = 0b000_00000000000 | ds4.ly;
            var b = 0b001_00000000000 | ds4.lx;
            var c = 0b010_00000000000 | ds4.ry;
            var d = 0b011_00000000000 | ds4.rx;

            divide(a, out var a0, out var a1);
            divide(b, out var b0, out var b1);
            divide(c, out var c0, out var c1);
            divide(d, out var d0, out var d1);
            divide(e, out var e0, out var e1);

            return new[] {
                c0, c1,
                a0, a1,
                e0, e1,
                d0, d1,
                b0, b1,
            };
        }
    }
}
