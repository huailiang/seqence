#ifndef __skillsystem__
#define __skillsystem__

#include <iostream>
#include <unordered_map>
#include "../Util/util.hpp"
#include "../Component/Skill.hpp"
#include "../Entitas/ISystem.hpp"

namespace Entitas
{
	using namespace std;

	class SkillSystem {

	public:

		Skill* GetSkill(const char* name)
		{
			if (skills_dict.find(name) != skills_dict.end())
			{
				return skills_dict[name];
			}
			else
			{
				auto skill = new Skill();
				skill->Reset(name);
				auto pair = make_pair(name, skill);
				skills_dict.insert(pair);
			}
		}

	private:
		std::unordered_map<const char*, Skill*> skills_dict;
	};

}


#endif
