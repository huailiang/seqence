#ifndef  __util__
#define __util__

#include "vector3.hpp"
#include <string>

namespace Entitas
{

	class util
	{

	public:
        
        static void write();

		static int LoadPath(const char* name);

		static int LoadScene(const char* name);
		
        static std::string GetAssetPath(const char* name);
        
		static bool CircleAttack(float radius, vector3 attack, vector3 skill);

		static bool RectAttack(vector3 attacker, vector3 attacked, vector3 forward, float length, float width);

		static bool SectorAttack(vector3 attacker, vector3 attacked, vector3 forward, float angle, float raduis);

		static vector3 Angle2Forward(float angle);
	};
}

#endif // ! __util__
