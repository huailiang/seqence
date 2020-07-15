#ifndef  __xmonster__
#define __xmonster__

#include "XActor.hpp"


namespace Entitas
{

	class Monster : public XActor
	{

	public:
		void Reset(unsigned int id, int confid, float hp, float sp, float attack, float hit, Path* path)
		{
			XActor::Reset(uid, confid, hp, sp, attack, hit, path);
		}

	};

}

#endif
