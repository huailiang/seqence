#ifndef  __xmonster__
#define __xmonster__

#include "XActor.hpp"


namespace Entitas
{

	class Monster : public XActor {
	public:
        void Reset(unsigned int id, float hp, float sp, float attack, float hit, Path* path) {
			XActor::Reset(uid, hp, sp, attack, hit, path);
		}

	};

}


#endif
