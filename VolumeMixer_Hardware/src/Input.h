#include "arduino.h"

class Input {
private:
	int8_t _lastDirection[3];
	int32_t _position[3];
	uint8_t _taster;
	int32_t _speed[3];

public:
	void init();
	void update();
	int32_t getPosition(uint8_t);
	int8_t getLastDirection(uint8_t);
	uint8_t getTaster();
	int32_t getLastSpeed(uint8_t);
};

void re_isr_impuls0();
void re_isr_impuls1();
void re_isr_impuls2();