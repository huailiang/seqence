#ifndef  __engine__
#define __engine__

#include "Entitas/SystemContainer.hpp"

namespace Entitas
{
	void Initial(int rate);
	void Update(float delta);
	void Destroy();
}

#endif // ! __engine__