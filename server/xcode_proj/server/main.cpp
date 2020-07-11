#include <iostream>
#include "engine.hpp"

using namespace std;

bool flag_q;

int main(int argc, const char * argv[]) {
    const int fps = 3;
    Entitas::Initial(fps);
    int per = 1000 *1000 / fps;
    clock_t rawtime = 0;
    
    while (true)
    {
        if (flag_q)
        {
            Entitas::Destroy();
            break;
        }
        
        auto t = clock();
        auto delta = t - rawtime;
        if (delta >= per)
        {
            Entitas::Update(delta /(1000* 1000.0f));
            rawtime = clock();
        }
    }
    return 0;
}
