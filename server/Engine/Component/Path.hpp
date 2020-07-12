#include "../Util/vector3.hpp"
#include "../Util/util.hpp"

namespace Entitas
{

	class Path : public IComponent {
        
	public:
		void Reset(const char* name) {
            
            util::LoadPath(name, cnt, time, pos, rot);
		}

        size_t cnt;
        float* time;
		vector3* pos;
        float* rot;
	};


}
