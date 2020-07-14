#ifndef  __skill__
#define __skill__

#include "../Util/vector3.hpp"
#include "../Util/util.hpp"
#include "../Entitas/IComponent.hpp"

namespace Entitas
{

	class Skill : public IComponent {

	public:
		
		void Reset(const char* name) {
			util::LoadSkill(name, cnt, start, duration, shapes, arg, arg2,
				types, effect);
		}


		~Skill()
		{
			cnt = 0;
			delete[] start;
			delete[] duration;
			delete[] shapes;
			delete[] arg;
			delete[] arg2;
			delete[] types;
			delete[] effect;
		}

	private:
		size_t cnt;
		float* start, *duration;
		int* shapes;
		float* arg, *arg2;
		std::vector<const char*>* types;
		std::vector<float>* effect;
	};
}

#endif
