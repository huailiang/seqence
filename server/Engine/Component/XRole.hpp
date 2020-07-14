#ifndef  __xrole__
#define __xrole__

#include "XEntity.hpp"


namespace Entitas
{

	class Role : public XEntity {
	public:
		void Reset(unsigned int id, float hp, float sp, float attack, float hit) 
		{
			XEntity::Reset(uid, hp, sp, attack, hit);
		}
	};

}


#endif
