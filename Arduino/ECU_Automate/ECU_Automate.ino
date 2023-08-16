#include <TcBUTTON.h>
#include <TcPINOUT.h>
#include <Servo.h>
#define BUTTON_SELECTOR_AUTO_PIN 8

// ------------------ Input ------------------
void btnSelectorAutoChanged(bool pressed);
TcBUTTON btnAutoSelector(BUTTON_SELECTOR_AUTO_PIN, btnSelectorAutoChanged, NULL, NULL, TcBUTTON::ButtonMode::PULLUP, true);

#define BUTTON_SELECTOR_MANUAL_PIN 9
void btnSelectorManualChanged(bool pressed);
TcBUTTON btnManualSelector(BUTTON_SELECTOR_MANUAL_PIN, btnSelectorManualChanged, NULL, NULL, TcBUTTON::ButtonMode::PULLUP, true);

#define BUTTON_START_PIN 5
void btnStartPressed(void);
void btnStartReleased(void);
TcBUTTON btnStart(BUTTON_START_PIN, btnStartPressed, btnStartReleased);

#define SERVO_PIN 4
Servo servo;
// ------------------ Output ------------------
#define ACC_PIN 44
TcPINOUT acc(ACC_PIN);

#define BAT_PIN 42
TcPINOUT bat(BAT_PIN);

#define PIK_PIN 38
TcPINOUT pik(PIK_PIN);

#define BRIGHT_PIN 40
TcPINOUT bright(BRIGHT_PIN);

#define MOTOR_PIN 48
TcPINOUT motor(MOTOR_PIN);

#define SPD_PIN 34
TcPINOUT spd(SPD_PIN);

#define REV_PIN 36
TcPINOUT rev(REV_PIN);
#define TRNR_PIN 32
TcPINOUT trnr(TRNR_PIN);

#define TRNL_PIN 30
TcPINOUT trnl(TRNL_PIN);

#define SOLENOID_PIN 46
TcPINOUT solenoid(SOLENOID_PIN);

#define MES_PIN 50
TcPINOUT mes(MES_PIN);

#define SERVO_PIN 4
// ------------------- Variable -------------------
const byte UpdateModeNone[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x40, 0x03 };
const byte UpdateModeAuto[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x41, 0x03 };
const byte UpdateModeManual[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x42, 0x03 };

const byte ResponseModeNone[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x40, 0x03 };
const byte ResponseModeAuto[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x41, 0x03 };
const byte ResponseModeManual[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x42, 0x03 };

const byte CommandStart[8] = { 0x02, 0x43, 0X49, 0x53, 0x00, 0x00, 0x00, 0x03 };

unsigned long lastTimeBtnStart = 0;
int countBtnStart = 0;
const int LENGTH = 8;
byte data[LENGTH];
int dataIndex = 0;
bool startReceived = false;

void serialEvent() {
  while (Serial.available()) {
    byte incomingByte = Serial.read();
    if (incomingByte == 0x02) {  // Start byte
      startReceived = true;
      dataIndex = 0;
    } else if (startReceived) {
      data[dataIndex++] = incomingByte;
      // End byte
      if (dataIndex == LENGTH - 1 && incomingByte == 0x03) {
        // Check and process the data
        // Ask for Mode Auto or Manual
        if (data[0] == 0x02 && data[1] == 0x51 && data[2] == 0x4D) {
          // Check Selector Button
          if (btnAutoSelector.getState() || btnManualSelector.getState()) {
            if (btnAutoSelector.getState()) {
              Serial.write(ResponseModeAuto, sizeof(ResponseModeAuto));
            } else {
              Serial.write(ResponseModeManual, sizeof(ResponseModeManual));
            }
          } else {
            Serial.write(ResponseModeNone, sizeof(ResponseModeNone));
          }
        }

        if (data[1] == 0x51 && data[2] == 0x4D) {
          DecodeData(data);
        }
        startReceived = false;
      } else if (dataIndex >= LENGTH) {
        startReceived = false;  // Reset if the message is longer than expected
      }
    }
  }
}

void setup() {
  Serial.begin(115200);
  // Serial.println("Start");
  servo.attach(SERVO_PIN);
  servo.write(0);
  // Update
  if (btnAutoSelector.getState() || btnManualSelector.getState()) {
    if (btnAutoSelector.getState()) {
      Serial.write(ResponseModeAuto, sizeof(ResponseModeAuto));
    } else {
      Serial.write(ResponseModeManual, sizeof(ResponseModeManual));
    }
  } else {
    Serial.write(ResponseModeNone, sizeof(ResponseModeNone));
  }
}

void loop() {
  btnAutoSelector.update();
  btnManualSelector.update();
  btnStart.update();
  btnStartOnPush();
}

void btnSelectorAutoChanged(bool pressed) {
  if (pressed) {
    Serial.write(UpdateModeAuto, sizeof(UpdateModeAuto));
  } else {
    Serial.write(UpdateModeNone, sizeof(UpdateModeNone));
  }
}

void btnSelectorManualChanged(bool pressed) {
  if (pressed) {
    Serial.write(UpdateModeManual, sizeof(UpdateModeManual));
  } else {
    Serial.write(UpdateModeNone, sizeof(UpdateModeNone));
  }
}

void btnStartPressed(void) {
  countBtnStart++;
}
bool IsSetStartOnPush = false;
void btnStartReleased(void) {
  if (countBtnStart == 1) {
    lastTimeBtnStart = millis();
    IsSetStartOnPush = true;
  }
}

void btnStartOnPush(void) {
  if (millis() - lastTimeBtnStart > 500 && IsSetStartOnPush) {
    if (countBtnStart == 1) {
      Serial.write(CommandStart, sizeof(CommandStart));
      Serial.println("Start " + String(countBtnStart));
    } else {
      byte data[8];
      memcpy(data, CommandStart, sizeof(CommandStart));  // This line copies the data from CommandStart to data array
      data[6] = (byte)countBtnStart;
      Serial.write(data, sizeof(data));
      Serial.println("Stop " + String(countBtnStart));
    }

    countBtnStart = 0;
    IsSetStartOnPush = false;
    lastTimeBtnStart = millis();
  }
}

void DecodeData(byte data[]) {
  // Check Command
  if (data[1] = 0x43 && data[2] == 0x49 && data[3] == 0x50) {
    // IO TYPE COMMAND
    switch (data[4]) {
      // 44
      case 0x2C:
        if (data[6] == 0x01) {
          acc.on();
        } else if (data[6] == 0x00) {
          acc.off();
        }
        break;
      // 42
      case 0x2A:
        if (data[6] == 0x01) {
          bat.on();
        } else if (data[6] == 0x00) {
          bat.off();
        }
        break;
      // 38
      case 0x26:
        if (data[6] == 0x01) {
          pik.on();
        } else if (data[6] == 0x00) {
          pik.off();
        }
        break;
      // 40
      case 0x28:
        if (data[6] == 0x01) {
          bright.on();
        } else if (data[6] == 0x00) {
          bright.off();
        }
        break;
      // 48
      case 0x30:
        if (data[6] == 0x01) {
          motor.on();
        } else if (data[6] == 0x00) {
          motor.off();
        }
        break;
      // 34
      case 0x22:
        if (data[6] == 0x01) {
          spd.on();
        } else if (data[6] == 0x00) {
          spd.off();
        }
        break;
      // 36
      case 0x24:
        if (data[6] == 0x01) {
          rev.on();
        } else if (data[6] == 0x00) {
          rev.off();
        }
        break;
      // 32
      case 0x20:
        if (data[6] == 0x01) {
          trnr.on();
        } else if (data[6] == 0x00) {
          trnr.off();
        }
        break;
      // 30
      case 0x1E:
        if (data[6] == 0x01) {
          trnl.on();
        } else if (data[6] == 0x00) {
          trnl.off();
        }
        break;
      // 4 Servo
      case 0x04:
        uint8_t value = data[6];
        servo.write(value);
        break;
      // 46
      case 0x2E:
        if (data[6] == 0x01) {
          solenoid.on();
        } else if (data[6] == 0x00) {
          solenoid.off();
        }
        break;
      // 50
      case 0x32:
        if (data[6] == 0x01) {
          mes.on();
        } else if (data[6] == 0x00) {
          mes.off();
        }
        break;
    }
  }
}