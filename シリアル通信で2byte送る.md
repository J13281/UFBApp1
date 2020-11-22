# シリアル通信で2byte送る

14bit送る。

送りたいデータ
00xxxxxxxyyyyyyy

分割する。
high -> 1xxxxxxx
low  -> 0yyyyyyy

send(c#)
```cs
static void Main(string[] args)
{
    var serial = new SerialPort
    {
        PortName = "COM3",
        BaudRate = 9600,
        DtrEnable = true
    };
    serial.Open();

    m1(12345, out var h1, out var l1);
    m1(16383, out var h2, out var l2);
    m1(00753, out var h3, out var l3);
    m1(00159, out var h4, out var l4);

    var output = new byte[] {
        h1, l1, h2, l2, h3, l3, h4, l4
    };

    serial.Write(output, 0, output.Length);

    var response = new byte[0xFF];
    while (true)
    {
        var len = serial.Read(response, 0, response.Length);
        if (len < 0) continue;
        for (int i = 0; i < len; i++)
        {
            Console.Write((char)response[i]);
        }
    }
}

static void m1(int n, out byte high, out byte low)
{
    high = (byte)(1 << 7 | n >> 7);
    low = (byte)(n & 0b1111111);
}
```

recieve(arduino)
```c++
void setup() {
  Serial.begin(9600);
}

int b, c;
void loop() {
  if (Serial.available()) {
    if (m1(Serial.read(), &b, &c)) {
      logic(c);
    }
  }
}

bool m1(int a, int* b, int* c) {
  if (a >> 7 & 1) {
    *b = a;
    return false;
  } else if (*b >> 7 & 1) {
    *c = (*b & 0b1111111) << 7 | (a & 0b1111111);
    *b = 0;
    return true;
  } else {
    return false;
  }
}

void logic(int d) {
  Serial.print("value is ");
  Serial.print(d);
  Serial.print(".\n");
}
```

result
```
value is 12345.
value is 16383.
value is 753.
value is 159.
```