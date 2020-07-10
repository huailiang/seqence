#include "../Util/vector3.hpp"
#include "../Entitas/Pool.hpp"


namespace Entitas
{

	class Position : public IComponent {
	public:
		void Reset(vector3 p) {
			pos = p;
		}

		vector3 pos;
	};
}