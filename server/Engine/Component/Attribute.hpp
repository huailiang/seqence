#include "../Entitas/Pool.hpp"


namespace Entitas
{

	class Attribute : public IComponent {
	public:
		void Reset(float hp, float sp, float attack, float hit) {
			this->hp = hp;
			this->sp = sp;
			this->attack = attack;
			this->hit = hit;
		}

		float hp;
		float sp;
		float attack;
		float hit;
	};

}