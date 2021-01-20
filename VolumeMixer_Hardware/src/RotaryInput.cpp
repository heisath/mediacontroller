#include "RotaryInput.h"



volatile int32_t _re_position0;
volatile uint8_t _re_pinMaskA;
volatile uint8_t _re_pinMaskB;

volatile int32_t _re_position1;
volatile uint8_t _re_pinMaskC;
volatile uint8_t _re_pinMaskD;

volatile int32_t _re_position2;
volatile uint8_t _re_pinMaskE;
volatile uint8_t _re_pinMaskF;

volatile uint8_t _re_tastMask0;
volatile uint8_t _re_tastMask1;
volatile uint8_t _re_tastMask2;
volatile uint8_t _re_taster;

#define PIN_RE_A		7
#define PIN_RE_B		6

#define PIN_RE_C		3
#define PIN_RE_D		5

#define PIN_RE_E		2
#define PIN_RE_F		4

#define PIN_T_0		8
#define PIN_T_1		9
#define PIN_T_2		10



void RotaryInput::Init() {


	pinMode(PIN_RE_A, INPUT_PULLUP);
	pinMode(PIN_RE_B, INPUT_PULLUP);
	pinMode(PIN_RE_C, INPUT_PULLUP);
	pinMode(PIN_RE_D, INPUT_PULLUP);
	pinMode(PIN_RE_E, INPUT_PULLUP);
	pinMode(PIN_RE_F, INPUT_PULLUP);

	_re_pinMaskA = digitalPinToBitMask(PIN_RE_A); //| digitalPinToBitMask(PIN_RE_B);
	_re_pinMaskB = digitalPinToBitMask(PIN_RE_B);

	_re_pinMaskC = digitalPinToBitMask(PIN_RE_C); //| digitalPinToBitMask(PIN_RE_B);
	_re_pinMaskD = digitalPinToBitMask(PIN_RE_D);

	_re_pinMaskE = digitalPinToBitMask(PIN_RE_E); //| digitalPinToBitMask(PIN_RE_B);
	_re_pinMaskF = digitalPinToBitMask(PIN_RE_F);

	attachInterrupt(digitalPinToInterrupt(PIN_RE_A), re_isr_impuls0, CHANGE);
	attachInterrupt(digitalPinToInterrupt(PIN_RE_C), re_isr_impuls1, CHANGE);
	attachInterrupt(digitalPinToInterrupt(PIN_RE_E), re_isr_impuls2, CHANGE);

	pinMode(PIN_T_0, INPUT_PULLUP);
	pinMode(PIN_T_1, INPUT_PULLUP);
	pinMode(PIN_T_2, INPUT_PULLUP);

	PCICR = 1 << PCIE0;
	PCMSK0 = (1 << PCINT6) | (1 << PCINT5) | (1 << PCINT4);
	SREG |= 1 << SREG_I;

	_re_tastMask0 = digitalPinToBitMask(PIN_T_0);
	_re_tastMask1 = digitalPinToBitMask(PIN_T_1);
	_re_tastMask2 = digitalPinToBitMask(PIN_T_2);

	_taster = 0;
	for (int i = 0; i < 3; i++) {
		_speed[i] = 0;
		_position[i] = 0;
		_lastDirection[i] = 0;
	}
}

void RotaryInput::Update() {

	int32_t newPos[3];
	newPos[0] = _re_position1 / 2;
	newPos[1] = _re_position0 / 2;
	newPos[2] = _re_position2 / 2;

	for (int i = 0; i < 3; i++) {
		if ((newPos[i] - _position[i]) > 0)
			_lastDirection[i] = 1;
		else if ((newPos[i] - _position[i]) < 0)
			_lastDirection[i] = -1;
		else
			_lastDirection[i] = 0;

		_speed[i] += (newPos[i] - _position[i]);
		_position[i] = newPos[i];
	}
	_taster = _re_taster;

}

uint8_t RotaryInput::GetButton() {
	uint8_t a = _taster;
	_taster = 0;
	_re_taster = 0;
	return a;
}

int8_t RotaryInput::GetLastDirection(uint8_t index) {
	int8_t a = _lastDirection[index];
	_lastDirection[index] = 0;
	return a;
}
int32_t RotaryInput::GetPosition(uint8_t index) {
	return _position[index];
}
int32_t RotaryInput::GetLastSpeed(uint8_t index) {
	int32_t a = _speed[index];
	_speed[index] = 0;
	return a;
}


ISR(PCINT0_vect)
{
	int t = PINB;

	if ((t & _re_tastMask0) == 0) 
		_re_taster |= 1 << 1;
	if ((t & _re_tastMask1) == 0) 
		_re_taster |= 1 << 0;
	if ((t & _re_tastMask2) == 0) 
		_re_taster |= 1 << 2;

}

void re_isr_impuls0() {
	int a = (PINE & _re_pinMaskA) | (PIND & _re_pinMaskB);

	if ((a == (_re_pinMaskA|_re_pinMaskB)) || (a == 0))
		_re_position0--;
	else
		_re_position0++;
}
void re_isr_impuls1() {
	int a = (PIND & _re_pinMaskC) | (PINC & _re_pinMaskD);

	if ((a == (_re_pinMaskC | _re_pinMaskD)) || (a == 0))
		_re_position1--;
	else
		_re_position1++;
}
void re_isr_impuls2() {
	int a = (PIND & _re_pinMaskE) | (PIND & _re_pinMaskF);

	if ((a == (_re_pinMaskE | _re_pinMaskF)) || (a == 0))
		_re_position2--;
	else
		_re_position2++;
}
