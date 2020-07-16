#ifndef  __path__
#define __path__

#include "../Util/vector3.hpp"
#include "../Entitas/IComponent.hpp"

namespace Entitas
{

	class Path : public IComponent {
        
	public:

		void Reset(const char* name);
        
		vector3 Sample(float t, float& r) const;

		~Path();

    private:
        size_t cnt;
        float* time;
		vector3* pos;
        float* rot;
	};
}

#endif
