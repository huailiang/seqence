#ifndef  __pathsystem__
#define __pathsystem__

#include <iostream>
#include <unordered_map>
#include "../Component/Path.hpp"
#include "../Entitas/ISystem.hpp"


namespace Entitas
{

	class PathSystem  {


	public:

		Path* Get(const char* name)
		{
			if (pat_dict.find(name) != pat_dict.end())
			{
				return pat_dict[name];
			}
			else
			{
				auto path = new Path();
				path->Reset(name);
				auto pair = std::make_pair(name, path);
				pat_dict.insert(pair);
			}
            return nullptr;
		}

	private:
		std::unordered_map<const char*, Path*> pat_dict;
	};
}

#endif
