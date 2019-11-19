#include "Input.h"

volatile int32_t _re_position;
volatile uint8_t _re_pinMaskA;
volatile uint8_t _re_pinMaskB;
volatile uint8_t _re_tastMaskL;
volatile uint8_t _re_tastMaskR;
volatile uint8_t _re_tastMaskC;
volatile uint8_t _re_taster;

#define PIN_RE_A		7
#define PIN_RE_B		6

#define PIN_T_LEFT		8
#define PIN_T_RIGHT		9
#define PIN_T_CENTER	10


void Input::init() {


	pinMode(PIN_RE_A, INPUT_PULLUP);
	pinMode(PIN_RE_B, INPUT_PULLUP);

	_re_pinMaskA = digitalPinToBitMask(PIN_RE_A); //| digitalPinToBitMask(PIN_RE_B);
	_re_pinMaskB = digitalPinToBitMask(PIN_RE_B);

	attachInterrupt(digitalPinToInterrupt(PIN_RE_A), re_isr_impuls, CHANGE);

	pinMode(PIN_T_LEFT, INPUT_PULLUP);
	pinMode(PIN_T_RIGHT, INPUT_PULLUP);
	pinMode(PIN_T_CENTER, INPUT_PULLUP);

	PCICR = 1 << PCIE0;
	PCMSK0 = (1 << PCINT6) | (1 << PCINT5) | (1 << PCINT4);
	SREG |= 1 << SREG_I;

	_re_tastMaskL = digitalPinToBitMask(PIN_T_LEFT);
	_re_tastMaskR = digitalPinToBitMask(PIN_T_RIGHT);
	_re_tastMaskC = digitalPinToBitMask(PIN_T_CENTER);

	_speed = 0;
	_position = 0;
	_taster = 0;
	_lastDirection = 0;
}

void Input::update() {
	long newPos = (_re_position / 2);

	if ((newPos - _position) > 0)
		_lastDirection = 1;
	else if ((newPos - _position) < 0)
		_lastDirection = -1;
	else 
		_lastDirection = 0;

	_speed += (newPos - _position);
	_position = newPos;
	_taster = _re_taster;
}

uint8_t Input::getTaster() {
	uint8_t a = _taster;
	_taster = 0;
	_re_taster = 0;
	return a;
}

int8_t Input::getLastDirection() {
	int8_t a = _lastDirection;
	_lastDirection = 0;
	return a;
}
int32_t Input::getPosition() {
	return _position;
}
int32_t Input::getLastSpeed() {
	int32_t a = _speed;
	_speed = 0;
	return a;
}


ISR(PCINT0_vect)
{
	int t = PINB;

	if ((t & _re_tastMaskL) == 0) 
		_re_taster |= 1 << 2;
	if ((t & _re_tastMaskC) == 0) 
		_re_taster |= 1 << 1;
	if ((t & _re_tastMaskR) == 0) 
		_re_taster |= 1 << 0;

}

void re_isr_impuls() {
	int a = (PINE & _re_pinMaskA) | (PIND & _re_pinMaskB);

	if ((a == (_re_pinMaskA|_re_pinMaskB)) || (a == 0))
		_re_position++;
	else
		_re_position--;
}
