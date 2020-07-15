#ifndef  __xentity__
#define __xentity__


#include "../Component/Path.hpp"
#include "../Entitas/IComponent.hpp"

namespace Entitas
{

	class XActor : public IComponent {
	public:
		void Reset(unsigned int id, int confId, float hp, float sp, float attack, float hit, Path* path) {
            this->uid = id;
			this->confId = confId;
			this->hp = hp;
			this->sp = sp;
			this->attack = attack;
			this->hit = hit;
		}

        unsigned int uid;
		int confId;
		float hp;
		float sp;
		float attack;
		float hit;
        Path* path;
        
	};

}


#endif
