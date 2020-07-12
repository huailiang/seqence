#include "interface.hpp"

namespace Entitas
{


    // c# -> c++
    void InitNative(SyncPos pos,SyncRot rot, SyncPlay play, BroadCast broad)
    {
        posDelegate = pos;
        rotDelegate = rot;
        playDelegate = play;
        broadDelegate = broad;
    }

    void NativeRecv(unsigned int id, unsigned char* buffer, int len)
    {
        
    }

}
