#ifndef  __pathsystem__
#define __pathsystem__

#include <iostream>
#include <unordered_map>
#include "../Singleton.hpp"
#include "../Component/Path.hpp"
#include "../Entitas/ISystem.hpp"


namespace Entitas
{

	class PathSystem : public Singleton<PathSystem>
	{

	public:
		void Get(const char* name, Path*& path)
		{
			if (pat_dict.find(name) != pat_dict.end())
			{
				path = pat_dict[name];
			}
			else
			{
				path = new Path();
				path->Reset(name);
				auto pair = std::make_pair(name, path);
				pat_dict.insert(pair);
			}
		}


	private:
		std::unordered_map<const char*, Path*> pat_dict;
	};
}

#endif
