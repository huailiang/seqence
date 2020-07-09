#ifndef  __util__
#define __util__

#include "../Component/Position.hpp"


namespace Entitas
{
	class util
	{
	public:
		void loadPath(const char* path);

		bool circleAttack(float radius, Position attack, Position skill);

	private:

	};
}

#endif // ! __util__