#include "arduino.h"

class Input {
private:
	int8_t _lastDirection;
	int32_t _position;
	uint8_t _taster;
	int32_t _speed;

public:
	void init();
	void update();
	int32_t getPosition();
	int8_t getLastDirection();
	uint8_t getTaster();
	int32_t getLastSpeed();
};

void re_isr_impuls();