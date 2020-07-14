#include <iostream>
#include "engine.hpp"

using namespace std;

bool flag_q;


int main(int argc, const char * argv[]) {
    const int fps = 3;
    const char* assets="../../../../Engine/assets/";
    Entitas::EngineInitial(fps,assets);
    int per = 1000 *1000 / fps;
    clock_t rawtime = 0;
    
    while (true)
    {
        if (flag_q)
        {
            Entitas::EngineDestroy();
            break;
        }

        auto t = clock();
        auto delta = t - rawtime;
        if (delta >= per)
        {
            Entitas::EngineUpdate(delta /(1000* 1000.0f));
            rawtime = clock();
        }
    }
    return 0;
}
