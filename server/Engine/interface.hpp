#ifndef  __interface__
#define  __interface__

#include "EngineInfo.hpp"


namespace Entitas
{
    
#if defined(__CYGWIN32__)
#define ENGINE_INTERFACE_EXPORT __declspec(dllexport)
#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
#define ENGINE_INTERFACE_EXPORT __declspec(dllexport)
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__) || defined(__QNX__)
#define ENGINE_INTERFACE_EXPORT
#else
#define ENGINE_INTERFACE_EXPORT
#endif
    
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
    ENGINE_INTERFACE_EXPORT void InitNative(SyncPos pos,SyncRot rot, SyncPlay play, BroadCast broad);
    
    ENGINE_INTERFACE_EXPORT void NativeRecv(unsigned int id, unsigned char* buffer, int len);
}

#endif // ! __interface__
