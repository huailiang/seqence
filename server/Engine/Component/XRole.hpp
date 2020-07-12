#ifndef  __xrole__
#define __xrole__

#include "XEntity.hpp"


namespace Entitas
{

	class Role : public XEntity {
	public:
		void Reset(unsigned int id, float hp, float sp, float attack, float hit) {
            this->uid = id;
			this->hp = hp;
			this->sp = sp;
			this->attack = attack;
			this->hit = hit;
		}

        unsigned int uid;
		float hp;
		float sp;
		float attack;
		float hit;
	};

}


#endif
