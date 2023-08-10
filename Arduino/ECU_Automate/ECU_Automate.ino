#include <TcBUTTON.h>
#include <TcPINOUT.h>
#include <Servo.h>
#define BUTTON_SELECTOR_AUTO_PIN 8


void btnSelectorAutoChanged(bool pressed);
TcBUTTON btnAutoSelector(BUTTON_SELECTOR_AUTO_PIN, btnSelectorAutoChanged, NULL, NULL,TcBUTTON::ButtonMode::PULLUP, true);

#define BUTTON_SELECTOR_MANUAL_PIN 9
void btnSelectorManualChanged(bool pressed);
TcBUTTON btnManualSelector(BUTTON_SELECTOR_MANUAL_PIN, btnSelectorManualChanged, NULL, NULL,TcBUTTON::ButtonMode::PULLUP, true);

#define LED_PIN 44
void ledChanged(bool pressed);
TcPINOUT led(LED_PIN, ledChanged);

#define SERVO_PIN 4
Servo servo;

const byte UpdateModeNone[8] = {0x02,0x55,0x4D,0x00,0x00,0x00,0x40,0x03};
const byte UpdateModeAuto[8] = {0x02,0x55,0x4D,0x00,0x00,0x00,0x41,0x03};
const byte UpdateModeManual[8] = {0x02,0x55,0x4D,0x00,0x00,0x00,0x42,0x03};

const byte ResponseModeNone[8] = {0x02,0x52,0x4D,0x00,0x00,0x00,0x40,0x03};
const byte ResponseModeAuto[8] = {0x02,0x52,0x4D,0x00,0x00,0x00,0x41,0x03};
const byte ResponseModeManual[8] = {0x02,0x52,0x4D,0x00,0x00,0x00,0x42,0x03};

void serialEvent() {
  while (Serial.available() > 7) 
  {
    byte data[8];
    for (int i = 0; i < 8; i++) {
      data[i] = Serial.read();
    }
    // Ask for Mode Auto or Manual
    if (data[0] == 0x02 && data[1] == 0x51 && data[2] == 0x4D && data[7] == 0x03) {
     // Check Selector Button 
      if (btnAutoSelector.getState()) {
        Serial.write(ResponseModeAuto, sizeof(ResponseModeAuto));
      } else if (btnManualSelector.getState()) {
        Serial.write(ResponseModeManual, sizeof(ResponseModeManual));
      } else {
        Serial.write(ResponseModeNone, sizeof(ResponseModeNone));
        }
    }
    //--------------------------------------------------------------------------------------
  }
}

void setup() {
  Serial.begin(115200);
  Serial.println("Start");
  // servo.attach(SERVO_PIN); 
}

void loop() {
  btnAutoSelector.update();
  btnManualSelector.update();
}
void ledChanged(bool pressed) {
  if (pressed) {
    Serial.println("Led on");
  } else {
    Serial.println("Led off");
  }
}

void btnSelectorAutoChanged(bool pressed){
  if (pressed) {
    Serial.write(UpdateModeAuto, sizeof(UpdateModeAuto));
  } else {
    Serial.write(UpdateModeNone, sizeof(UpdateModeNone));
  }
}

void btnSelectorManualChanged(bool pressed){
  if (pressed) {
    Serial.write(UpdateModeManual, sizeof(UpdateModeManual));
  } else {
    Serial.write(UpdateModeNone, sizeof(UpdateModeNone));
  }
}