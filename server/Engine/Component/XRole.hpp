#ifndef  __xrole__
#define __xrole__

#include "XActor.hpp"


namespace Entitas
{

	class Role : public XActor {

	public:

		void Reset(unsigned int id, float hp, float sp, float attack, float hit, Path* path)
		{
			XActor::Reset(uid, hp, sp, attack, hit,path);
		}

	};

}


#endif
