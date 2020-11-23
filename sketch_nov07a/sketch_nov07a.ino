#include <SPI.h>

void setup() {

  // SPIの初期化処理を行う
  SPI.begin();                        // ＳＰＩを行う為の初期化
  SPI.setBitOrder(MSBFIRST);          // ビットオーダー
  SPI.setClockDivider(SPI_CLOCK_DIV8);// クロック(CLK)をシステムクロックの1/8で使用(16MHz/8)
  SPI.setDataMode(SPI_MODE0);         // クロック極性０(LOW)　クロック位相０

  pinMode(2, OUTPUT); digitalWrite(2, HIGH);
  pinMode(3, OUTPUT); digitalWrite(3, HIGH);
  pinMode(4, OUTPUT); digitalWrite(4, HIGH);
  pinMode(5, OUTPUT); digitalWrite(5, HIGH);
  pinMode(6, OUTPUT); digitalWrite(6, HIGH);
  pinMode(7, OUTPUT); digitalWrite(7, HIGH);
  pinMode(8, OUTPUT); digitalWrite(8, HIGH);
  pinMode(9, OUTPUT); digitalWrite(9, HIGH);
  pinMode(A0, OUTPUT); digitalWrite(A0, HIGH);
  pinMode(A1, OUTPUT); digitalWrite(A1, HIGH);

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

// 000_00000000000
void logic(int d) {
  int head = d >> 11;
  int body = d & 0b11111111111;

  if (head == 0) {
    int p = 0x3000 | map(body, 0, 0x7FF, 0, 0xFFF);
    mcp4922_out(A0, p);
  } else if (head == 1) {
    int p = 0xB000 | map(body, 0, 0x7FF, 0, 0xFFF);
    mcp4922_out(A0, p);
  } else if (head == 2) {
    int p = 0x3000 | map(body, 0, 0x7FF, 0, 0xFFF);
    mcp4922_out(A1, p);
  } else if (head == 3) {
    int p = 0xB000 | map(body, 0, 0x7FF, 0, 0xFFF);
    mcp4922_out(A1, p);
  } else if (head == 4) {
    digitalWrite(2, body >> 0 & 1);
    digitalWrite(3, body >> 1 & 1);
    digitalWrite(4, body >> 2 & 1);
    digitalWrite(5, body >> 3 & 1);
    digitalWrite(6, body >> 4 & 1);
    digitalWrite(7, body >> 5 & 1);
    digitalWrite(8, body >> 6 & 1);
    digitalWrite(9, body >> 7 & 1);
  }
}

void mcp4922_out(int ss_pin, int data) {
  digitalWrite(ss_pin, LOW);

  // Highバイト(0x30=OUTA/BUFなし/1x/シャットダウンなし)
  SPI.transfer(data >> 8);
  // Lowバイトの出力
  SPI.transfer(data & 0xff);

  digitalWrite(ss_pin, HIGH);
}
