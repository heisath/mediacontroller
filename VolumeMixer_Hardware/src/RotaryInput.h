#include "arduino.h"

class RotaryInput {
private:
	int8_t _lastDirection[3];
	int32_t _position[3];
	uint8_t _taster;
	int32_t _speed[3];

public:
	void Init();
	void Update();
	int32_t GetPosition(uint8_t);
	int8_t GetLastDirection(uint8_t);
	uint8_t GetButton();
	int32_t GetLastSpeed(uint8_t);
};

void re_isr_impuls0();
void re_isr_impuls1();
void re_isr_impuls2();