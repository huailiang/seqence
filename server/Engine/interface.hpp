#ifndef  __interface__
#define  __interface__

#include "EngineInfo.hpp"

namespace Entitas
{
    // c++ -> c#
    typedef void (*SyncPos)(unsigned int id, float x, float y, float z);
    
    typedef void (*SyncRot)(unsigned int id, float rot);
    
    typedef void (*SyncPlay)(unsigned int id, const char* skillID);
    
    typedef void (*BroadCast)(unsigned char* buffer, int len);
   
    
    extern SyncPos posDelegate;
    extern SyncRot rotDelegate;
    extern SyncPlay  playDelegate;
    extern BroadCast broadDelegate;
    
    // c# -> c++
    void InitNative(SyncPos pos,SyncRot rot, SyncPlay play, BroadCast broad);
    
    void NativeRecv(unsigned int id, unsigned char* buffer, int len);
}

#endif // ! __interface__
