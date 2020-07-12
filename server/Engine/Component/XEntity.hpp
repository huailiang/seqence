#ifndef  __xentity__
#define __xentity__


namespace Entitas
{

	class XEntity : public IComponent {
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
