#include <iostream>
#include <string>
#include <time.h>
#include <vector>
#include "engine.hpp"
#include "conio.h"

using namespace std;



int main(int argc, char* argv[])
{
	const int fps = 30;
	const char* assets = "../Engine/assets/";
	Entitas::EngineInitial(fps, assets);
	int per = 1000 / fps;
	clock_t rawtime = 0;
	int ch;
	while (true)
	{
		if (_kbhit()) {
			ch = _getch();
			cout << ch;
			if (ch == 27) { // key ESC presed, ESC value = 27
				Entitas::EngineDestroy();
				break;
			}
		}
		auto t = clock();
		int delta = t - rawtime;
		if (delta >= per)
		{
			Entitas::EngineUpdate(delta / 1000.0f);
			rawtime = clock();
		}
	}
	return 1;
}