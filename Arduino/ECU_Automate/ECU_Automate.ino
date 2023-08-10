#include <TcBUTTON.h>
#include <TcPINOUT.h>
#include <Servo.h>
#define BUTTON_SELECTOR_PIN 5


void buttonSelectorChanged(bool pressed);
TcBUTTON buttonSelector(BUTTON_SELECTOR_PIN, buttonSelectorChanged, NULL, NULL);

#define LED_PIN 30
void ledChanged(bool pressed);
TcPINOUT led(LED_PIN, ledChanged);

#define SERVO_PIN 4
Servo servo;

const byte ModeAuto[5] = {0x02,0x4D,0x30,0x41,0x03};
const byte ModeManual[5] = {0x02,0x4D,,0x30,0x42,0x03};

void serialEvent() {
  while (Serial.available() > 7) {
    byte data[8];
    for (int i = 0; i < 8; i++) {
      data[i] = Serial.read();
    }
    Serial.print("Data: ");
    for (int i = 0; i < 8; i++) {
      Serial.print(data[i], HEX);
      Serial.println(); // New line
    }
    // if (data[0] == 0x02 && data[7] == 0x03) {
    //   if (data[6] == 0x41) {
    //     // Serial Print Mode Auto
    //     Serial.write(ModeAuto, sizeof(ModeAuto));
    //   } else if (data[6] == 0x42) {
    //     // Serial Print Mode Manual
    //     Serial.write(ModeManual, sizeof(ModeManual));
    //   }
    // }
  }
}

void setup() {
  Serial.begin(115200);
  Serial.println("Start");
  // servo.attach(SERVO_PIN); 
}

void loop() {
  // buttonSelector.update();
  // led.toggle();
  // servo.write(180);
  delay(5000);
  // servo.write(0);
}
void buttonSelectorChanged(bool pressed) {
  if (pressed) {
    Serial.println("Button pressed");
    // Serial Print Mode Auto
    Serial.write(ModeAuto, sizeof(ModeAuto));
  } else {
    Serial.println("Button pressed");
    // Serial Print Mode Manual
    Serial.write(ModeManual, sizeof(ModeManual));
  }
}

void ledChanged(bool pressed) {
  if (pressed) {
    Serial.println("Led on");
  } else {
    Serial.println("Led off");
  }
}