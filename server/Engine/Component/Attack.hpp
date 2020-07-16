#ifndef  __skill__
#define __skill__

#include <vector>
#include "../EngineInfo.hpp"
#include "../Entitas/IComponent.hpp"


namespace Entitas
{

	class Attack : public IComponent {

	public:

		void Reset(const char* name);

		size_t Find(float t);

		~Attack();

		size_t cnt;
		float time;
		float* start, *duration;
		int* shapes;
		float* arg, *arg2;
		std::vector<const char*>* types;
		std::vector<float>* effect;
	};
}

#endif
