#ifndef  __engine__
#define __engine__


#include "Entitas/SystemContainer.hpp"

namespace Entitas
{
	void EngineInitial(int rate, const char* assets);
	
    void EngineUpdate(float delta);

	void EngineNotify(unsigned int id, unsigned char* buffer, int len);
    
	void EngineDestroy();
}

#endif // ! __engine__
