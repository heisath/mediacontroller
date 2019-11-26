/*
 Name:		VolumeMixer_Hardware.ino
 Created:	11/15/2019 4:19:15 PM
 Author:	Jannis
*/

#include <Adafruit_ILI9341.h>
#include "src\Input.h"
#include <Adafruit_GFX.h>
#include <XPT2046_Touchscreen.h>
#include <SPI.h>

// For the Adafruit shield, these are the default.

#define TFT_DC A3
#define TFT_RS A2
#define TFT_CS A1
#define CS_PIN A0

class Page {
public:
	char title[21];
	char type[3];
	uint8_t value;
};

// Use hardware SPI (on Uno, #13, #12, #11) and the above for CS/DC
Adafruit_ILI9341 display = Adafruit_ILI9341(TFT_CS, TFT_DC, TFT_RS);
XPT2046_Touchscreen ts(CS_PIN);
Input input_re;
Page pages[3];
Page oldpages[3];

const int rowheight = 80;
const int offset = 16;

void predraw() {
	for (int i = 0; i < 3; i++)
	{
		display.drawRect(9, offset + 24 + i * rowheight, 302, 10, ILI9341_WHITE);
		display.setCursor(0, offset + i * rowheight);
		display.print((char)17);
		display.setCursor(300, offset + i * rowheight);
		display.print((char)16);
	}
	
}

void redraw(int i = -1, int v_only = 0) {
	if (i == -1) {
		for (int n = 0; n < 3; n++)
		{
			redraw(n);
		}
		return;
	}

	Page* page = (pages + i);
	if (v_only == 0) {
		display.fillRect(20, offset + i * rowheight, 280, 16, ILI9341_BLACK);

		display.setCursor(20, offset + i * rowheight);
		display.setTextColor(ILI9341_WHITE);
		display.print(page->type);
		display.print(page->title);
	}

	display.fillRect(10, offset + 24 + i * rowheight + 1, page->value*3, 8, ILI9341_GREEN);
	display.fillRect(10 + page->value*3, offset + 24 + i * rowheight + 1, 300 - page->value*3, 8, ILI9341_BLACK);
}


// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);

	display.begin();
	display.setRotation(1);
	display.setTextSize(2);
	display.setTextColor(ILI9341_WHITE);
	display.setTextWrap(false);
	display.fillScreen(ILI9341_BLACK);

	ts.begin();
	ts.setRotation(1);

	predraw();
	redraw(-1);

	// Init Rotary Encoder
	input_re.init();

}

uint32_t lastTastCheck = millis();
uint32_t lastTouched = millis();

// the loop function runs over and over again until power down or reset
void loop() {
	Page* page;
	while (Serial.available() > 0) {
		String nr = Serial.readStringUntil(':');
		String sesType = Serial.readStringUntil(':');
		String val = Serial.readStringUntil(':');
		String name = Serial.readStringUntil('\n');
		
		int i = atoi(nr.c_str()); 
		page = (pages + i);
		if (sesType == "1")
			sprintf(page->type, "%s", "M:");
		else if (sesType == "2") 
			sprintf(page->type, "%s", "C:");
		else 
			sprintf(page->type, "%s", "_:");

		memset(page->title, 0, 21);
		memcpy(page->title, name.c_str(), name.length());
		page->value = atoi(val.c_str());

		redraw(i);
	}


	input_re.update();
	
	for (int i = 0; i < 3; i++) {
		int8_t lastDir = input_re.getLastDirection(i);
		int32_t speed = input_re.getLastSpeed(i);

		page = pages + i;

		while (speed > 0) {
			speed--;
			if (page->value < 100) {
				page->value++;
				Serial.print(":VOL");
				Serial.print(i);
				Serial.print(":");
				Serial.println(page->value);

				redraw(i, 1);
			}
		}
		while (speed < 0) {
			speed++;
			if (page->value > 0) {
				page->value--;
				Serial.print(":VOL");
				Serial.print(i);
				Serial.print(":");
				Serial.println(page->value);

				redraw(i, 1);
			}
		}
	}

	if (millis() - lastTastCheck > 500) {
		uint8_t inputT = input_re.getTaster();

		if ((inputT & 1 << 0) != 0) {
			Serial.print(":RES_VOL");
			Serial.println(0);
		}
		else if ((inputT & 1 << 1) != 0) {
			Serial.print(":RES_VOL");
			Serial.println(1);
		}
		else if ((inputT & 1 << 2) != 0) {
			Serial.print(":RES_VOL");
			Serial.println(2);
		}
		lastTastCheck = millis();
	}
	
	boolean istouched = ts.touched();
	if (istouched && (millis() - lastTouched) > 250 ) {
		TS_Point p = getPoint();
		int row = p.y / 80;
		if (p.x < 80) {
			Serial.print(":PREV_PAGE:");
			Serial.println(row);
		}else if (p.x > 240) {
			Serial.print(":NEXT_PAGE:");
			Serial.println(row);
		}
		else {
			redraw();
		}

		lastTouched = millis();

	}

	delay(30);
}

TS_Point getPoint() {
	TS_Point p = ts.getPoint();
	p.x = min(map(p.x, 190, 3800, 0, 320), 319);
	p.y = min(map(p.y, 250, 3900, 0, 240), 239);
	if (p.x < 0) p.x = 0;
	if (p.y < 0) p.y = 0;
	return p;
}

