#include "../Entitas/Pool.hpp"


namespace Entitas
{

	class Position : public IComponent {
	public:
		void Reset(float px, float py, float pz) {
			x = px;
			y = py;
			z = pz;
		}

		float x;
		float y;
		float z;
	};
}