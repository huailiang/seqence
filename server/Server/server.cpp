#include <iostream>
#include <string>
#include <time.h>
#include <vector>
#include "engine.hpp"
#include "conio.h"

using namespace std;

bool flag_q;


class AB
{
public:
	AB(int _a)
	{
		a = _a;
	}

	~AB()
	{
		printf("DESTROY \n");
	}

	int a;
};

std::shared_ptr<AB> ptr;
std::vector< std::shared_ptr<AB>> arr;

void test()
{
	auto ptr = std::shared_ptr<AB>(new AB(2));
	cout << ptr->a << endl;
	arr.push_back(ptr);
	ptr = nullptr;
};

int main()
{
	cout << "server start" << endl;
	test();
	cout << "size: " << arr.size() << endl;
	arr.clear();
	cout << "size2:" << arr.size() << endl;

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