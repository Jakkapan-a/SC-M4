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
unsigned long lastTimeServoPosition = 0;
uint8_t servoPosition = 100;
uint8_t servoPositionOld = 100;
uint8_t speedServo = 200;
const uint8_t servoDiscrepancy = 20;
const uint8_t servoSlow = 40;
const uint8_t servoFast = 5;
uint8_t GetSpeed(uint8_t servoPositionOld, uint8_t servoPosition);

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
      data[dataIndex++] = incomingByte;
    } else if (startReceived) {
      data[dataIndex++] = incomingByte;
      // End byte
      if (dataIndex >= LENGTH - 1 && incomingByte == 0x03) {
        // Check and process the data
        // Ask for Mode Auto or Manual
        if (data[1] == 0x51 && data[2] == 0x4D) {
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

        if (data[1] == 0x43 && data[2] == 0x49) {
          // Serial.write(14);
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
  servo.attach(SERVO_PIN);
  // servo.write(servoPosition);
  //  solenoid.on();
}

void loop() {
  btnAutoSelector.update();
  btnManualSelector.update();
  btnStart.update();
  btnStartOnPush();
  ServoControl();
}

void ServoControl() {
  // -------------------- Servo --------------------
  if (servoPosition != servoPositionOld) {
    if (millis() - lastTimeServoPosition > GetSpeed(servoPositionOld, servoPosition)) {
      if (servoPosition > servoPositionOld) {
        servoPositionOld++;
      } else {
        servoPositionOld--;
      }
      servo.write(servoPositionOld);
      lastTimeServoPosition = millis();
    }
  }
  // -------------------- Servo --------------------
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
    } else {
      byte data[8];
      memcpy(data, CommandStart, sizeof(CommandStart));  // This line copies the data from CommandStart to data array
      data[6] = (byte)countBtnStart;
      Serial.write(data, sizeof(data));
    }
    countBtnStart = 0;
    IsSetStartOnPush = false;
    lastTimeBtnStart = millis();
  }
}

uint8_t GetSpeed(uint8_t old_input, uint8_t new_input) {
  uint8_t speed = servoSlow;
  if (old_input > new_input + servoDiscrepancy) {
    speed = servoFast;
  } else if (old_input < new_input - servoDiscrepancy) {
    speed = servoFast;
  } else {
    speed = servoSlow;
  }
  return speed;
}
void DecodeData(byte data[]) {
  // Check Command
  if (data[1] == 0x43 && data[2] == 0x49 && data[3] == 0x50) {
    
    byte command = data[4];
    byte action = data[6];

    if (command == 0x2C) { // 44
      if (action == 0x01) acc.on();
      else if (action == 0x00) acc.off();
    }
    else if (command == 0x2A) { // 42
      if (action == 0x01) bat.on();
      else if (action == 0x00) bat.off();
    }
    else if (command == 0x26) { // 38
      if (action == 0x01) pik.on();
      else if (action == 0x00) pik.off();
    }
    else if (command == 0x28) { // 40
      if (action == 0x01) bright.on();
      else if (action == 0x00) bright.off();
    }
    else if (command == 0x30) { // 48
      if (action == 0x01) motor.on();
      else if (action == 0x00) motor.off();
    }
    else if (command == 0x22) { // 34
      if (action == 0x01) spd.on();
      else if (action == 0x00) spd.off();
    }
    else if (command == 0x24) { // 36
      if (action == 0x01) rev.on();
      else if (action == 0x00) rev.off();
    }
    else if (command == 0x20) { // 32
      if (action == 0x01) trnr.on();
      else if (action == 0x00) trnr.off();
    }
    else if (command == 0x1E) { // 30
      if (action == 0x01) trnl.on();
      else if (action == 0x00) trnl.off();
    }
    else if (command == 0x04) { // 4 Servo
      uint8_t value = action;
      if (value > 180) servoPosition = 180;
      else if (value < 0) servoPosition = 0;
      else servoPosition = value;
    }
    else if (command == 0x2E) { // 46
      if (action == 0x01) solenoid.on();
      else if (action == 0x00) solenoid.off();
    }
    else if (command == 0x32) { // 50
      if (action == 0x01) mes.on();
      else if (action == 0x00) mes.off();
    }
    else {
      Serial.write(command);
    }
  }
}


// void DecodeData(byte data[]) {
//   // Check Command
//   if (data[1] = 0x43 && data[2] == 0x49 && data[3] == 0x50) {                                                       
//     switch (data[4]) {
//       case 0x2C:  // 44
//         if (data[6] == 0x01) {
//           acc.on();
//         } else if (data[6] == 0x00) {
//           acc.off();
//         }
//         break;
//       case 0x2A:  // 42
//         if (data[6] == 0x01) {
//           bat.on();
//         } else if (data[6] == 0x00) {
//           bat.off();
//         }
//         break;
//       case 0x26:  // 38
//         if (data[6] == 0x01) {
//           pik.on();
//         } else if (data[6] == 0x00) {
//           pik.off();
//         }
//         break;
//       case 0x28:  // 40
//         if (data[6] == 0x01) {
//           bright.on();
//         } else if (data[6] == 0x00) {
//           bright.off();
//         }
//         break;
//       case 0x30:  // 48
//         if (data[6] == 0x01) {
//           motor.on();
//         } else if (data[6] == 0x00) {
//           motor.off();
//         }
//         break;
//       case 0x22:  // 34
//         if (data[6] == 0x01) {
//           spd.on();
//         } else if (data[6] == 0x00) {
//           spd.off();
//         }
//         break;
//       case 0x24:  // 36
//         if (data[6] == 0x01) {
//           rev.on();
//         } else if (data[6] == 0x00) {
//           rev.off();
//         }
//         break;
//       case 0x20:  // 32
//         if (data[6] == 0x01) {
//           trnr.on();
//         } else if (data[6] == 0x00) {
//           trnr.off();
//         }
//         break;
//       case 0x1E:  // 30
//         if (data[6] == 0x01) {
//           trnl.on();
//         } else if (data[6] == 0x00) {
//           trnl.off();
//         }
//         break;
//       case 0x04:  // 4 Servo
//         uint8_t value = data[6];
//         if (value > 180) {
//           servoPosition = 180;
//         } else if (value < 0) {
//           servoPosition = 0;
//         } else {
//           servoPosition = value;
//         }
//         break;
//       case 0x2E:  // 46
//         Serial.write(data[4]);
//         if (data[6] == 0x01) {
//           solenoid.on();
//         } else if (data[6] == 0x00) {
//           solenoid.off();
//         }
//         break;
//       case 0x32:  // 50
//         if (data[6] == 0x01) {
//           mes.on();
//         } else if (data[6] == 0x00) {
//           mes.off();
//         }
//         break;
//       default:
//         Serial.write(data[4]);
//         break;
//     }
//   }
// }