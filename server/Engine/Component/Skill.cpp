#include "Skill.hpp"
#include "../Util/vector3.hpp"
#include "../Util/util.hpp"


namespace Entitas
{

	void Skill::Reset(const char* name) {
		util::LoadSkill(name, cnt, start, duration, shapes, arg, arg2,
			types, effect);
		time = EngineInfo::time;
	}

	size_t Skill::Find(float t)
	{
		float tick = t - time;
		if (cnt > 0) // bounds
		{
			if (tick <= start[0])
			{
				return 0;
			}
			else
			{
				int end = cnt - 1;
				float last = start[end] + duration[end];
				if (tick > last)
				{
					return end;
				}
			}
		}
		for (size_t i = 0; i < cnt; i++)
		{
			if (start[i] <= tick && start[i] + duration[i] > tick)
			{
				return i;
			}
		}
		return -1;
	}


	Skill::~Skill()
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

}
