// 
// 
// 

#include "VolumeMixer.h"

const int rowcount = 3;
const int rowheight = 60;

const int sb_rowheight = 240 / 4;
const int sb_rowwidth = 320 / 4;
const int offset = 16;

enum PageTypes {
	PAGE_VOLUME,
	PAGE_SOUNDBOARD
};

// Use hardware SPI and the above for CS/DC
Adafruit_ILI9341 display = Adafruit_ILI9341(TFT_CS, TFT_DC, TFT_RS);
XPT2046_Touchscreen ts(TCH_CS);
RotaryInput rotary;

// Define rows for volume control
VolumeRow rows[rowcount];
VolumeRow oldRows[rowcount];
VolumeRow* volume_row;

int selected_output_config = 0;
int selected_page = PAGE_VOLUME;

// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);

	display.begin();
	display.setRotation(1);
	display.setTextSize(2);
	display.setTextColor(ILI9341_WHITE);
	display.setTextWrap(false);

	ts.begin();
	ts.setRotation(1);

	InitVolumePage();
	

	// Init Rotary Encoder
	rotary.Init();

}

uint32_t lastTastCheck = millis();
uint32_t lastTouched = millis();
uint32_t currentTime = millis();
uint32_t newTime = millis();

// the loop function runs over and over again until power down or reset
void loop() {
	
	while (Serial.available() > 0) {
		String nr = Serial.readStringUntil(':');

		if (nr == "SET_OUT") {
			String val = Serial.readStringUntil('\n');
			selected_output_config = atoi(val.c_str());

			if (selected_page == PAGE_VOLUME) DrawDeviceSelector();
		}
		else {
			String sesType = Serial.readStringUntil(':');
			String val = Serial.readStringUntil(':');
			String name = Serial.readStringUntil('\n');

			int i = atoi(nr.c_str());
			volume_row = (rows + i);
			if (sesType == "1")
				sprintf(volume_row->type, "%s", "M:");
			else if (sesType == "2")
				sprintf(volume_row->type, "%s", "C:");
			else
				sprintf(volume_row->type, "%s", "_:");

			int v_only = 1;
			if (strcmp(volume_row->title, name.c_str()) != 0) {
				memset(volume_row->title, 0, 21);
				memcpy(volume_row->title, name.c_str(), name.length());
				v_only = 0;
			}

			volume_row->value = atoi(val.c_str());

			if (selected_page == PAGE_VOLUME) DrawVolumeSlider(i, v_only);
		}
	}


	rotary.Update();

	bool istouched = ts.touched() && ((millis() - lastTouched) > 250);
	TS_Point p;
	if (istouched) {
		p = getPoint();
		lastTouched = millis();
	}

	uint8_t inputT = 0;
	if (millis() - lastTastCheck > 500) {
		inputT = rotary.GetButton();
		lastTastCheck = millis();
	}

	if (selected_page == PAGE_VOLUME) 
	{
		UpdateVolumePage(istouched, p, inputT);
	}
	else if (selected_page == PAGE_SOUNDBOARD)
	{
		UpdateSoundboardPage(istouched, p);
	}

	newTime = millis();
	if (newTime - currentTime < 25)
		delay(30 - (newTime - currentTime));

	currentTime = millis();
}

void ChangePage() {
	if (selected_page == PAGE_SOUNDBOARD) {
		selected_page = PAGE_VOLUME;
		InitVolumePage();
	}
	else if (selected_page == PAGE_VOLUME) {
		selected_page = PAGE_SOUNDBOARD;
		InitSoundboardPage();
	}
}

void InitSoundboardPage() {
	display.fillScreen(ILI9341_BLACK);

	for (int y = 1; y < 4; y++)
	{
		display.drawFastHLine(0, y * sb_rowheight, 320, ILI9341_WHITE);
	}
	for (int x = 1; x < 4; x++)
	{
		display.drawFastVLine(x * sb_rowwidth,0, 240, ILI9341_WHITE);
	}
}


inline void UpdateSoundboardPage(bool istouched, TS_Point p) {
	if (istouched) {
		int button = (p.x / sb_rowwidth) + 4 * (p.y / sb_rowheight);

		if (button == 15)
		{
			ChangePage();
		}
		else {
			Serial.print("SB_PRESS");
			Serial.println(button);
		}
	}
}


void InitVolumePage() {
	display.fillScreen(ILI9341_BLACK);

	for (int i = 0; i < rowcount; i++)
	{
		display.drawRect(9, offset + 24 + i * rowheight, 302, 10, ILI9341_WHITE);
		display.setCursor(0, offset + i * rowheight);
		display.print((char)17);
		display.setCursor(310, offset + i * rowheight);
		display.print((char)16);
	}

	display.setCursor(10, offset + 3 * rowheight + 5);
	display.print("Mixed      H600      G935");


	for (int n = 0; n < rowcount; n++)
	{
		DrawVolumeSlider(n);
	}

	DrawDeviceSelector();
}
inline void UpdateVolumePage(bool istouched, TS_Point p, uint8_t inputT) {
	for (int i = 0; i < 3; i++) {
		int8_t lastDir = rotary.GetLastDirection(i);
		int32_t speed = rotary.GetLastSpeed(i);

		if (lastDir != 0) {

			volume_row = rows + i;

			int32_t newValue = (int32_t)volume_row->value + speed;
			newValue = min(100, newValue);
			newValue = max(0, newValue);

			volume_row->value = (uint8_t)newValue;

			Serial.print("VOL");
			Serial.print(i);
			Serial.print(":");
			Serial.println(volume_row->value);

			DrawVolumeSlider(i, 1);
		}
	}

	if (inputT != 0) {
		if ((inputT & 1 << 0) != 0) {
			Serial.print("RES_VOL");
			Serial.println(0);
		}
		else if ((inputT & 1 << 1) != 0) {
			Serial.print("RES_VOL");
			Serial.println(1);
		}
		else if ((inputT & 1 << 2) != 0) {
			Serial.print("RES_VOL");
			Serial.println(2);
		}
	}

	if (istouched) {
		int row = p.y / 60;
		if (row < rowcount) {
			if (p.x < 80) {
				Serial.print("PREV_PAGE");
				Serial.println(row);
			}
			else if (p.x > 240) {
				Serial.print("NEXT_PAGE");
				Serial.println(row);
			}
			else {
				ChangePage();
			}
		}
		else {
			selected_output_config = p.x / 106;
			Serial.print("SEL_OUT");
			Serial.println(selected_output_config);
			DrawDeviceSelector();
		}
	}
}

void DrawVolumeSlider(int i, int v_only = 0) {
	volume_row = (rows + i);
	if (v_only == 0) {
		display.fillRect(20, offset + i * rowheight, 280, 16, ILI9341_BLACK);

		display.setCursor(20, offset + i * rowheight);
		display.setTextColor(ILI9341_WHITE);
		display.print(volume_row->type);
		display.print(volume_row->title);
	}
	display.startWrite();
	display.writeFillRect(10, offset + 24 + i * rowheight + 1, volume_row->value * 3, 8, ILI9341_GREEN);
	display.writeFillRect(10 + volume_row->value * 3, offset + 24 + i * rowheight + 1, 300 - volume_row->value * 3, 8, ILI9341_BLACK);
	display.endWrite();
}

void DrawDeviceSelector() {
	display.fillRect(10, offset + 3 * rowheight + 5, 300, 15, ILI9341_BLACK);
	display.setCursor(10, offset + 3 * rowheight + 5);

	switch (selected_output_config)
	{
	case 0:
		display.print(">Mixed<    H600     G935");
		break;
	case 1:
		display.print(" Mixed    >H600<    G935");
		break;
	case 2:
		display.print(" Mixed     H600    >G935<");
		break;
	}

}

TS_Point getPoint() {
	TS_Point p = ts.getPoint();
	p.x = min(map(p.x, 190, 3800, 0, 320), 319);
	p.y = min(map(p.y, 250, 3900, 0, 240), 239);
	if (p.x < 0) p.x = 0;
	if (p.y < 0) p.y = 0;
	return p;
}
