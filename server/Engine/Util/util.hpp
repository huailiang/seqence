#ifndef  __util__
#define __util__

#include "vector3.hpp"
#include "tinyxml2.h"
#include <string>
#include <unordered_map>

namespace Entitas
{

	class util
	{

	public:

		util();

		static int LoadPath(const char* name, size_t& cnt, float*& time, vector3*& pos, float*& rot);

		static int LoadSkill(const char* name, size_t& cnt, float* start, float* duration, int* shapes,
			float* arg, float* arg2, std::vector<const char*>* types, std::vector<float>* effect);

		static std::string GetAssetPath(const char* name);

		static bool CircleAttack(float radius, vector3 attack, vector3 skill);

		static bool RectAttack(vector3 attacker, vector3 attacked, vector3 forward, float length, float width);

		static bool SectorAttack(vector3 attacker, vector3 attacked, vector3 forward, float angle, float raduis);

		static vector3 Angle2Forward(float angle);

		static unsigned int GetIncUID();

		template<typename T>
		static T lerp(T x1, T x2, float t)
		{
			return x1 + (x2 - x1) * t;
		}

	private:

		static unsigned int inc_uid;

	};
}

#endif // ! __util__
