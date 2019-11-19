/*
 Name:		VolumeMixer_Hardware.ino
 Created:	11/15/2019 4:19:15 PM
 Author:	Jannis
*/

#include "src\Input.h"
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

Adafruit_SSD1306 display(-1);
Input input_re;

#if (SSD1306_LCDHEIGHT != 64)
#error("Height incorrect, please fix Adafruit_SSD1306.h!");
#endif



// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);
	Serial.println("Hallo");
	display.begin(SSD1306_SWITCHCAPVCC, 0x3c);  // initialize with the I2C addr 0x3D (for the 128x64)

	display.setTextSize(1);
	display.setTextColor(WHITE);
	display.clearDisplay();

	display.print("Hallo");
	display.display();

	// Init Rotary Encoder
	input_re.init();

}


String currentSessionType = "";
String currentPageTitle = "";
uint8_t currentValue = 0;

// the loop function runs over and over again until power down or reset
void loop() {
	
	if (Serial.available() > 0) {
		String sesType = Serial.readStringUntil(':');
		String val = Serial.readStringUntil(':');
		String line = Serial.readStringUntil('\n');
		
		if (sesType == "1")
			currentSessionType = "Multimedia";
		else if (sesType == "2") 
			currentSessionType = "Communication";
		else 
			currentSessionType = "Console";

		currentPageTitle = line;
		
		currentValue = atoi(val.c_str());
	}


	input_re.update();
	
	uint8_t inputT = input_re.getTaster();
	int8_t lastDir = input_re.getLastDirection();
	int32_t speed = input_re.getLastSpeed();

	if ((inputT & 1<<0) != 0) {
		Serial.println(": NEXT_PAGE");
	}
	else if ((inputT & 1<<2) != 0) {
		Serial.println(": PREV_PAGE");
	}
	
	if ((inputT & 1 << 1) != 0) {
		Serial.println(": RES_VOL");
	}

	while(speed > 0) {
		speed--;
		if (currentValue < 100) {
			currentValue++;
			Serial.print(": VOL");
			Serial.println(currentValue);
		}
	}
	while(speed < 0) {
		speed++;
		if (currentValue > 0) {
			currentValue--;
			Serial.print(": VOL");
			Serial.println(currentValue);
		}
	}

	display.clearDisplay();
	display.setCursor(0, 2);
	display.print(currentSessionType);

	display.setCursor(0, 16);
	display.print(currentPageTitle);
		
	display.drawRect(2, 56, 100, 8, 1);
	display.fillRect(2, 56, currentValue, 8, 1);
	display.setCursor(104, 56);
	display.print(currentValue);
	
	display.display();

	delay(100);

	
}


