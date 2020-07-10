#include <vector>
#include "util.hpp"
#include "../Component/Position.hpp"
using namespace std;

namespace Entitas
{

	void util::LoadPath(const char* path)
	{
		FILE *fp = fopen(path, "w");
		fprintf(fp, "你要写入txt的内容");
		fclose(fp);
	}

	bool util::CircleAttack(float radius, vector3 attack, vector3 skill)
	{
		float distance = (attack - skill).length();
		return distance <= radius;
	}

	bool util::RectAttack(vector3 attacker, vector3 attacked, float length, float width)
	{
		

		return true;
	}

	bool util::SectorAttack(vector3 attacker, vector3 attacked, float angle, float raduis)
	{
		vector3 delta = attacked - attacker;
		return true;
	}


}