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

void loop() {
  if (Serial.available()) {
    logic(Serial.read());
  }
}

void logic(int state) {

  int head = state >> 5;
  int body = state & 0x1F;

  if (head == 0) {
    digitalWrite(2, state >> 0 & 1);
    digitalWrite(3, state >> 1 & 1);
    digitalWrite(4, state >> 2 & 1);
    digitalWrite(5, state >> 3 & 1);
  }
  else if (head == 1) {
    digitalWrite(6, state >> 0 & 1);
    digitalWrite(7, state >> 1 & 1);
    digitalWrite(8, state >> 2 & 1);
    digitalWrite(9, state >> 3 & 1);
  }
  else if (head == 2) {
    // digitalWrite(A0, state >> 0 & 1);
    // digitalWrite(A1, state >> 1 & 1);
    // digitalWrite(A2, state >> 2 & 1);
    // digitalWrite(A3, state >> 3 & 1);
  }
  else if (head == 3) {
    // empty
  }
  else if (head == 4) {
    // ly
    int p = 0xB000 | map(body, 0, 30, 0, 0xFFF);
    mcp4922_out(A0, p);
  }
  else if (head == 5) {
    // lx
    int p = 0x3000 | map(body, 0, 30, 0, 0xFFF);
    mcp4922_out(A0, p);
  }
  else if (head == 6) {
    // ry
    int p = 0xB000 | map(body, 0, 30, 0, 0xFFF);
    mcp4922_out(A1, p);
  }
  else if (head == 7) {
    // rx
    int p = 0x3000 | map(body, 0, 30, 0, 0xFFF);
    mcp4922_out(A1, p);
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
