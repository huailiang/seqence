#include "../Entitas/Pool.hpp"


namespace Entitas
{

	class Move : public IComponent
	{
	public:
		void Reset(float _speed) {
			speed = _speed;
		}

		float speed;
	};
}