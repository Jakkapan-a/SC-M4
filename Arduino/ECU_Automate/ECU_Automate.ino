#include <TcBUTTON.h>
#include <TcPINOUT.h>
#include <Servo.h>
#include <Adafruit_INA219.h>
//*********************** INPUT Sensor ***********************//
/* 
 * Up to 4 boards may be connected. Addressing is as follows:
 * Board 0: Address = 0x40 Offset = binary 00000 (no jumpers required)
 * Board 1: Address = 0x41 Offset = binary 00001 (bridge A0 as in the photo above)
 * Board 2: Address = 0x44 Offset = binary 00100 (bridge A1)
 * Board 3: Address = 0x45 Offset = binary 00101 (bridge A0 & A1)
*/

Adafruit_INA219 ina219_A(0x40);
Adafruit_INA219 ina219_V(0x41);

#define BUTTON_SELECTOR_AUTO_PIN 8

void ASK_ECU_VER(void);
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

#define BUTTON_RESET_PIN 10
void btnResetChanged(bool pressed);
TcBUTTON btnReset(BUTTON_RESET_PIN, btnResetChanged, NULL, NULL, TcBUTTON::ButtonMode::PULLUP, true);  // RESET SW VER

#define BUTTON_ASK_SOFTWARE_VER_PIN 26
void btnAskSoftwareVerPressed(void);
void btnAskSoftwareVerReleased(void);
TcBUTTON btnAskSoftwareVer(BUTTON_ASK_SOFTWARE_VER_PIN, btnAskSoftwareVerPressed, btnAskSoftwareVerReleased);

#define BUTTON_TOUCH_VIEW_PIN 28
void btnTouchViewPressed(void);
void btnTouchViewReleased(void);
TcBUTTON btnTouchView(BUTTON_TOUCH_VIEW_PIN, btnTouchViewPressed, btnTouchViewReleased);

#define SERVO_PIN 6
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

// ------------------- Variable -------------------
const byte UpdateModeNone[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x40, 0x03 };
const byte UpdateModeAuto[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x41, 0x03 };
const byte UpdateModeManual[8] = { 0x02, 0x55, 0x4D, 0x00, 0x00, 0x00, 0x42, 0x03 };
const byte UpdateStatusReset[8] = { 0x02, 0x55, 0x53, 0x52, 0x00, 0x00, 0x00, 0x03 };

const byte ResponseModeNone[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x40, 0x03 };
const byte ResponseModeAuto[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x41, 0x03 };
const byte ResponseModeManual[8] = { 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x42, 0x03 };

const byte CommandStart[8] = { 0x02, 0x43, 0X49, 0x53, 0x00, 0x00, 0x00, 0x03 };
// 10 04 84 04 C7 00 19 5A 03

byte TPMS_temps[9];
uint8_t view_mode_flag = 0;

unsigned long lastTimeBtnStart = 0;
unsigned long lastTimeServoPosition = 0;
unsigned long lastTimeUpdateCurrent = 0;
uint8_t servoPosition = 100;
uint8_t servoPositionOld = 100;
uint8_t speedServo = 200;
const uint8_t servoDiscrepancy = 30;
const uint8_t servoSlow = 20;
const uint8_t servoFast = 2;
uint8_t GetSpeed(uint8_t servoPositionOld, uint8_t servoPosition);

int countBtnStart = 0;
const int LENGTH = 8;
byte data[LENGTH];
int dataIndex = 0;
bool startReceived = false;
bool isUpdateSelectorStatus = false;
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
          //  delay(50);
          isUpdateSelectorStatus = true;
          lastTimeUpdateCurrent = millis();
        }

        if (data[1] == 0x43 && data[2] == 0x49) {
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
  Serial.begin(9600);
  Serial3.begin(9600);
  ina219_A.begin();
  ina219_V.begin();


  servo.attach(SERVO_PIN);
  servo.write(servoPosition);
}

void loop() {
  btnAutoSelector.update();
  btnManualSelector.update();
  btnStart.update();
  btnReset.update();
  btnAskSoftwareVer.update();
  btnTouchView.update();
  btnStartOnPush();
  ServoControl();
  UpdateCurrent();
}

union DoubleToBytes {
  double doubleVal;
  byte byteArray[4];
};

bool IsToggleCurrentVoltage = false;
uint8_t countCurrentUpdate = 0;
void UpdateCurrent() {
  if (millis() - lastTimeUpdateCurrent > 500) {

    IsToggleCurrentVoltage = !IsToggleCurrentVoltage;
    //---------------------------------Update current -----------------------------------------//
    countCurrentUpdate++;
    if(countCurrentUpdate >0){
      // Update Voltage B
      double voltage = ina219_A.getBusVoltage_V();
      // Check voltage is negative
      
      DoubleToBytes doubleToBytes;
      doubleToBytes.doubleVal = voltage;
      // 02 55 43 00 00 00 00 03
      byte data[8];
      data[0] = 0x02;
      data[1] = 0x55;
      data[2] = 0x56;
      data[3] = doubleToBytes.byteArray[0];
      data[4] = doubleToBytes.byteArray[1];
      data[5] = doubleToBytes.byteArray[2];
      data[6] = doubleToBytes.byteArray[3];
      data[7] = 0x03;
      Serial.write(data, sizeof(data));

    }else if(countCurrentUpdate > 1){
      // Update Amp B
      double current = ina219_A.getCurrent_mA();
      DoubleToBytes doubleToBytes;
      doubleToBytes.doubleVal = current;
      // 02 55 43 00 00 00 00 03
      byte data[8];
      data[0] = 0x02;
      data[1] = 0x55;
      data[2] = 0x43;
      data[3] = doubleToBytes.byteArray[0];
      data[4] = doubleToBytes.byteArray[1];
      data[5] = doubleToBytes.byteArray[2];
      data[6] = doubleToBytes.byteArray[3];
      data[7] = 0x03;
      Serial.write(data, sizeof(data));
    }else if(countCurrentUpdate > 2){
      // Update Voltage V
       double voltage = ina219_V.getBusVoltage_V();
      DoubleToBytes doubleToBytes;
      doubleToBytes.doubleVal = voltage;
      // 02 55 43 00 00 00 00 03
      byte data[8];
      data[0] = 0x02;
      data[1] = 0x55;
      data[2] = 0x57;
      data[3] = doubleToBytes.byteArray[0];
      data[4] = doubleToBytes.byteArray[1];
      data[5] = doubleToBytes.byteArray[2];
      data[6] = doubleToBytes.byteArray[3];
      data[7] = 0x03;
      Serial.write(data, sizeof(data));
    }else if(countCurrentUpdate > 3){
      // Update Amp V
      double current = ina219_V.getCurrent_mA();
      DoubleToBytes doubleToBytes;
      doubleToBytes.doubleVal = current;
      // 02 55 44 00 00 00 00 03
      byte data[8];
      data[0] = 0x02;
      data[1] = 0x55;
      data[2] = 0x44;
      data[3] = doubleToBytes.byteArray[0];
      data[4] = doubleToBytes.byteArray[1];
      data[5] = doubleToBytes.byteArray[2];
      data[6] = doubleToBytes.byteArray[3];
      data[7] = 0x03;
      Serial.write(data, sizeof(data));
      
      countCurrentUpdate = 0;
    }

    if (IsToggleCurrentVoltage) {
     
    } else {
     
    }
    //---------------------------------End Update current -----------------------------------------//

    if (isUpdateSelectorStatus) {
      if (btnAutoSelector.getState() || btnManualSelector.getState()) {

        if (btnAutoSelector.getState()) {
          // 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x41, 0x03 
          byte data[8];
          data[0] = 0x02;
          data[1] = 0x52;
          data[2] = 0x4D;
          data[3] = 0x00;
          data[4] = 0x00;
          data[5] = 0x00;
          data[6] = 0x41;
          data[7] = 0x03;
          Serial.write(data, sizeof(data));
        } else {
          // 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x42, 0x03
          byte data[8];
          data[0] = 0x02;
          data[1] = 0x52;
          data[2] = 0x4D;
          data[3] = 0x00;
          data[4] = 0x00;
          data[5] = 0x00;
          data[6] = 0x42;
          data[7] = 0x03;
          Serial.write(data, sizeof(data));
        }
      } else {
        // 0x02, 0x52, 0x4D, 0x00, 0x00, 0x00, 0x40, 0x03
        byte data[8];
        data[0] = 0x02;
        data[1] = 0x52;
        data[2] = 0x4D;
        data[3] = 0x00;
        data[4] = 0x00;
        data[5] = 0x00;
        data[6] = 0x40;
        data[7] = 0x03;
      }
      
      isUpdateSelectorStatus = false;
    }
    lastTimeUpdateCurrent = millis();
  }
}
void serial3Event() {
  while (Serial3.available()) {
    byte incomingByte = Serial3.read();
    // Serial.print(incomingByte,HEX);
  }
}
void ASK_ECU_VER(void) {
  uint8_t MCU_TO_Device_Data[9];
  // 10 04 84 04 C7 00 19 5A 03
  MCU_TO_Device_Data[0] = 0x10;
  MCU_TO_Device_Data[1] = 0x04;
  MCU_TO_Device_Data[2] = 0x84;
  MCU_TO_Device_Data[3] = 0x04;
  MCU_TO_Device_Data[4] = 0xC7;
  MCU_TO_Device_Data[5] = 0x00;
  MCU_TO_Device_Data[6] = 0x19;
  MCU_TO_Device_Data[7] = 0x5A;
  MCU_TO_Device_Data[8] = 0x03;
  Serial3.write(MCU_TO_Device_Data, sizeof(MCU_TO_Device_Data));
  delay(100);

  // 10 04 84 04 C5 00 22 63 03
  MCU_TO_Device_Data[0] = 0x10;
  MCU_TO_Device_Data[1] = 0x04;
  MCU_TO_Device_Data[2] = 0x84;
  MCU_TO_Device_Data[3] = 0x04;
  MCU_TO_Device_Data[4] = 0xC5;
  MCU_TO_Device_Data[5] = 0x00;
  MCU_TO_Device_Data[6] = 0x22;
  MCU_TO_Device_Data[7] = 0x63;
  MCU_TO_Device_Data[8] = 0x03;
  Serial3.write(MCU_TO_Device_Data, sizeof(MCU_TO_Device_Data));
  delay(100);

  // 10 04 84 04 CE 00 2A 60 03
  MCU_TO_Device_Data[0] = 0x10;
  MCU_TO_Device_Data[1] = 0x04;
  MCU_TO_Device_Data[2] = 0x84;
  MCU_TO_Device_Data[3] = 0x04;
  MCU_TO_Device_Data[4] = 0xCE;
  MCU_TO_Device_Data[5] = 0x00;
  MCU_TO_Device_Data[6] = 0x2A;
  MCU_TO_Device_Data[7] = 0x60;
  MCU_TO_Device_Data[8] = 0x03;
  Serial3.write(MCU_TO_Device_Data, sizeof(MCU_TO_Device_Data));
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
      // Serial.print("Servo POS:");
      // Serial.println(servoPositionOld);
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

void btnResetChanged(bool pressed) {
  if (pressed) {
    Serial.println("reset");
    byte data[8];
    memcpy(data, UpdateStatusReset, sizeof(UpdateStatusReset));
    data[6] = 0x01;
    Serial.write(data, sizeof(data));
  } else {
    byte data[8];
    memcpy(data, UpdateStatusReset, sizeof(UpdateStatusReset));
    data[6] = 0x02;
    Serial.write(data, sizeof(data));
  }
  mes.off();
}

void Touch_View_Func(uint8_t view_flag) {

  if (view_flag == 3) {
    TPMS_temps[0] = 0x10;
    TPMS_temps[1] = 0x04;
    TPMS_temps[2] = 0x84;
    TPMS_temps[3] = 0x00;
    TPMS_temps[4] = 0x57;
    TPMS_temps[5] = 0x02;
    TPMS_temps[6] = 0x85;
    TPMS_temps[7] = 0x50;
    TPMS_temps[8] = 0x03;
  } else if (view_flag == 0) {
    //10 04 84 00 E5 02 86 E1 03
    TPMS_temps[0] = 0x10;
    TPMS_temps[1] = 0x04;
    TPMS_temps[2] = 0x84;
    TPMS_temps[3] = 0x00;
    TPMS_temps[4] = 0xE5;
    TPMS_temps[5] = 0x02;
    TPMS_temps[6] = 0x86;
    TPMS_temps[7] = 0xE1;
    TPMS_temps[8] = 0x03;
  } else if (view_flag == 1) {
    TPMS_temps[0] = 0x10;
    TPMS_temps[1] = 0x04;
    TPMS_temps[2] = 0x84;
    TPMS_temps[3] = 0x01;
    TPMS_temps[4] = 0x80;
    TPMS_temps[5] = 0x02;
    TPMS_temps[6] = 0x90;
    TPMS_temps[7] = 0x93;
    TPMS_temps[8] = 0x03;
  } else if (view_flag == 2) {
    TPMS_temps[0] = 0x10;
    TPMS_temps[1] = 0x04;
    TPMS_temps[2] = 0x84;
    TPMS_temps[3] = 0x02;
    TPMS_temps[4] = 0x19;
    TPMS_temps[5] = 0x02;
    TPMS_temps[6] = 0x92;
    TPMS_temps[7] = 0x0B;
    TPMS_temps[8] = 0x03;
  }
  Serial3.write(TPMS_temps, sizeof(TPMS_temps));
}

void btnAskSoftwareVerPressed(void) {
}
void btnAskSoftwareVerReleased(void) {
  ASK_ECU_VER();
}

void btnTouchViewPressed(void) {
}
void btnTouchViewReleased(void) {
  Touch_View_Func(view_mode_flag);
  view_mode_flag++;
  if (view_mode_flag >= 4) {
    view_mode_flag = 0;
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

    if (command == 0x2C) {  // 44
      if (action == 0x01) acc.on();
      else if (action == 0x00) acc.off();
    } else if (command == 0x2A) {  // 42
      if (action == 0x01) bat.on();
      else if (action == 0x00) bat.off();
    } else if (command == 0x26) {  // 38
      if (action == 0x01) pik.on();
      else if (action == 0x00) pik.off();
    } else if (command == 0x28) {  // 40
      if (action == 0x01) bright.on();
      else if (action == 0x00) bright.off();
    } else if (command == 0x30) {  // 48
      if (action == 0x01) motor.on();
      else if (action == 0x00) motor.off();
    } else if (command == 0x22) {  // 34
      if (action == 0x01) spd.on();
      else if (action == 0x00) spd.off();
    } else if (command == 0x24) {  // 36
      if (action == 0x01) rev.on();
      else if (action == 0x00) rev.off();
    } else if (command == 0x20) {  // 32
      if (action == 0x01) trnr.on();
      else if (action == 0x00) trnr.off();
    } else if (command == 0x1E) {  // 30
      if (action == 0x01) trnl.on();
      else if (action == 0x00) trnl.off();
    } else if (command == 0x04) {  // 4 Servo
      uint8_t value = action;
      if (value > 180) servoPosition = 180;
      else if (value < 0) servoPosition = 0;
      else servoPosition = value;
    } else if (command == 0x2E) {  // 46
      if (action == 0x01) solenoid.on();
      else if (action == 0x00) solenoid.off();
    } else if (command == 0x32) {  // 50
      if (action == 0x01) {
        mes.on();
      } else if (action == 0x00) {
      }
    } else if (command == 0x18) {  // 24

    } else if (command == 0x1A) {  // 26
      if (action == 0x01) {
        btnTouchViewReleased();
      } else if (action == 0x00) {
      }

      // btnTouchViewReleased(void);

    } else if (command == 0x1C) {  // 28
      ASK_ECU_VER();
    }

    else {
      Serial.write(command);
    }
  }
}