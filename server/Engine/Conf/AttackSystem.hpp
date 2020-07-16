#ifndef __skillsystem__
#define __skillsystem__

#include <iostream>
#include <unordered_map>
#include "../Util/util.hpp"
#include "../Component/Attack.hpp"
#include "../Entitas/ISystem.hpp"

namespace Entitas
{
	using namespace std;

	class AttackSystem : public Singleton<AttackSystem> {

	public:

		Attack* Get(const char* name)
		{
			if (attack_dict.find(name) != attack_dict.end())
			{
				return attack_dict[name];
			}
			else
			{
				auto skill = new Attack();
				skill->Reset(name);
				auto pair = make_pair(name, skill);
				attack_dict.insert(pair);
                return skill;
			}
		}

	private:
		std::unordered_map<const char*, Attack*> attack_dict;
	};

}


#endif
