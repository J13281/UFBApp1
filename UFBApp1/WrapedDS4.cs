using SharpDX.DirectInput;
using System.Linq;

namespace Pad1
{
    class WrapedDS4
    {
        public Joystick ds4;
        public JoystickState state = new JoystickState();

        public bool left;
        public bool up;
        public bool right;
        public bool down;

        public bool btn0;
        public bool btn1;
        public bool btn2;
        public bool btn3;
        public bool l1;
        public bool l2;
        public bool r1;
        public bool r2;

        public int lx;
        public int ly;
        public int rx;
        public int ry;

        public static WrapedDS4 initFirst(InputRange range)
        {
            var ds4 = initFirst();
            ds4.ds4.GetObjectPropertiesByName("X").Range = range;
            ds4.ds4.GetObjectPropertiesByName("Y").Range = range;
            ds4.ds4.GetObjectPropertiesByName("Z").Range = range;
            ds4.ds4.GetObjectPropertiesByName("RotationZ").Range = range;
            return ds4;
        }

        public static WrapedDS4 initFirst()
        {
            var dinput = new DirectInput();

            var guid = dinput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
                .First().InstanceGuid;

            var joystick = new Joystick(dinput, guid);

            return new WrapedDS4(joystick);
        }

        public WrapedDS4(Joystick ds4)
        {
            this.ds4 = ds4;
        }

        public void Update()
        {
            ds4.Acquire();
            ds4.Poll();
            ds4.GetCurrentState(ref state);

            left = state.PointOfViewControllers[0] == 27000;
            up = state.PointOfViewControllers[0] == 0;
            right = state.PointOfViewControllers[0] == 9000;
            down = state.PointOfViewControllers[0] == 18000;

            btn0 = state.Buttons[0];
            btn1 = state.Buttons[3];
            btn2 = state.Buttons[2];
            btn3 = state.Buttons[1];

            l1 = state.Buttons[4];
            l2 = state.Buttons[6];
            r1 = state.Buttons[5];
            r2 = state.Buttons[7];

            lx = state.X;
            ly = state.Y;
            rx = state.Z;
            ry = state.RotationZ;
        }
    }
}
