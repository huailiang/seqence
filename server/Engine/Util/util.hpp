#ifndef  __util__
#define __util__

#include "vector3.hpp"
#include <string>

namespace Entitas
{

	class util
	{

	public:
        
        static int LoadPath(const char* name, size_t& cnt, float*& time, vector3*& pos, float*& rot);
		
        static std::string GetAssetPath(const char* name);
        
		static bool CircleAttack(float radius, vector3 attack, vector3 skill);

		static bool RectAttack(vector3 attacker, vector3 attacked, vector3 forward, float length, float width);

		static bool SectorAttack(vector3 attacker, vector3 attacked, vector3 forward, float angle, float raduis);

		static vector3 Angle2Forward(float angle);
        
        template<typename T>
        static T lerp(T x1, T x2, float t)
        {
            return x1 + (x2 - x1) * t;
        }
	};
}

#endif // ! __util__
