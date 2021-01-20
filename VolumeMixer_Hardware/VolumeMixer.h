// VolumeMixer.h

#ifndef _VOLUMEMIXER_h
#define _VOLUMEMIXER_h

	#include "arduino.h"
#endif

#include <Adafruit_ILI9341.h>
#include "src\RotaryInput.h"
#include <Adafruit_GFX.h>
#include <XPT2046_Touchscreen.h>
#include <SPI.h>

#define TFT_DC A3
#define TFT_RS A2
#define TFT_CS A1
#define TCH_CS A0

class VolumeRow {
public:
	char title[21];
	char type[3];
	uint8_t value;
};


TS_Point getPoint();

void ChangePage();


void InitVolumePage();
void DrawVolumeSlider(int i, int v_only = 0);
//void DrawVolumeSliderAll();
void DrawDeviceSelector();
inline void UpdateVolumePage(bool istouched, TS_Point p, uint8_t inputT);

void InitSoundboardPage();
inline void UpdateSoundboardPage(bool istouched, TS_Point p);