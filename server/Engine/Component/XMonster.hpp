#ifndef  __xmonster__
#define __xmonster__

#include "XEntity.hpp"


namespace Entitas
{

	class Monster : public XEntity {
	public:
        void Reset(unsigned int id, float hp, float sp, float attack, float hit) {
            this->uid = id;
			this->hp = hp;
			this->sp = sp;
			this->attack = attack;
			this->hit = hit;
		}

	};

}


#endif
