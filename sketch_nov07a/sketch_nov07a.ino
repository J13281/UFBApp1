void setup() {
  pinMode(2, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(16, OUTPUT);
  pinMode(14, OUTPUT);
  pinMode(15, OUTPUT);
  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
  pinMode(A2, OUTPUT);
  pinMode(A3, OUTPUT);
  digitalWrite(2, HIGH);
  digitalWrite(4, HIGH);
  digitalWrite(7, HIGH);
  digitalWrite(8, HIGH);
  digitalWrite(10, HIGH);
  digitalWrite(16, HIGH);
  digitalWrite(14, HIGH);
  digitalWrite(15, HIGH);
  digitalWrite(A0, HIGH);
  digitalWrite(A1, HIGH);
  digitalWrite(A2, HIGH);
  digitalWrite(A3, HIGH);
  Serial.begin(9600);
}

int n = 0;
void loop() {
  if (Serial.available()) {
    logic(Serial.read());
  }
}

void logic(int state) {

  int head = state >> 5;
  int body = state & 31;

  if (head == 0) {
    digitalWrite(2, state >> 0 & 1);
    digitalWrite(4, state >> 1 & 1);
    digitalWrite(7, state >> 2 & 1);
    digitalWrite(8, state >> 3 & 1);
  }
  else if (head == 1) {
    digitalWrite(10, state >> 0 & 1);
    digitalWrite(16, state >> 1 & 1);
    digitalWrite(14, state >> 2 & 1);
    digitalWrite(15, state >> 3 & 1);
  }
  else if (head == 2) {
    digitalWrite(A0, state >> 0 & 1);
    digitalWrite(A1, state >> 1 & 1);
    digitalWrite(A2, state >> 2 & 1);
    digitalWrite(A3, state >> 3 & 1);
  }
  else if (head == 3) {
    // empty
  }
  else if (head == 4) {
    // lx
    // analogWrite(3, map(body, 0, 31, 0, 255));
    analogWrite(3, 127);
  }
  else if (head == 5) {
    // ly
    // analogWrite(5, map(body, 0, 31, 0, 255));
    analogWrite(5, 127);
  }
  else if (head == 6) {
    // rx
    analogWrite(6, map(body, 0, 31, 0, 255));
  }
  else if (head == 7) {
    // ry
    analogWrite(9, map(body, 0, 31, 0, 255));
  }
}
