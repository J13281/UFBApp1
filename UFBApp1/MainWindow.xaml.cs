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
            ds4 = WrapedDS4.initFirst(new InputRange(0, 31));
            serial = new SerialPort { BaudRate = 9600, PortName = "COM3" };
            serial.Open();
        }

        void Loop()
        {
            ds4.Update();
            var bytes = Logics.m1(ds4);
            serial.Write(bytes, 0, bytes.Length);
        }
    }

    static class Logics
    {
        public static byte[] m1(WrapedDS4 ds4)
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
                var f = barray[i] ? 0 : 1;
                d[i / 4] |= (byte)(f << i % 4);
            }

            //d[4] |= (byte)(ds4.lx);
            //d[5] |= (byte)(ds4.ly);
            //d[6] |= (byte)(ds4.rx);
            //d[7] |= (byte)(ds4.ry);

            d[4] |= (byte)15;
            d[5] |= (byte)15;
            d[6] |= (byte)15;
            d[7] |= (byte)15;

            return d;
        }
    }
}
