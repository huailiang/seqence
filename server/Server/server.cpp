#include <iostream>
#include <string>
#include <time.h>
#include "engine.hpp"
#include "conio.h"

using namespace std;

bool flag_q;


int main()
{
	cout << "server start" << endl;
	const int fps = 30;
	Entitas::Initial();
	int per = 1000 / fps;
	clock_t rawtime = 0;
	int ch;
	while (true)
	{
		if (flag_q)
		{
			Entitas::Destroy();
			break;
		}
		
		if (_kbhit()) {
			ch = _getch();
			cout << ch;
			if (ch == 27) { //当按下ESC时循环，ESC键的键值时27.
				flag_q = true;
				break;
			}
		}
		auto t = clock();
		int delta = t - rawtime;
		if (delta >= per)
		{
			Entitas::Update(delta / 1000.0f);
			rawtime = clock();
		}
	}
	return 1;
}