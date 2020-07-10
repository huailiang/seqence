#ifndef  __util__
#define __util__

#include "vector3.hpp"


namespace Entitas
{

	class util
	{

	public:

		static int LoadPath(const char* path);

		static int LoadScene(const char* path);
		
		static bool CircleAttack(float radius, vector3 attack, vector3 skill);

		static bool RectAttack(vector3 attacker, vector3 attacked, vector3 forward, float length, float width);

		static bool SectorAttack(vector3 attacker, vector3 attacked, vector3 forward, float angle, float raduis);

	};
}

#endif // ! __util__