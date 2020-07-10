#include <vector>
#include <iostream>
#include "util.hpp"
#include "tinyxml2.h"


using namespace std;
using namespace tinyxml2;

namespace Entitas
{

	const float PI = 3.1415926;


	int util::LoadPath(const char* path)
	{
		tinyxml2::XMLDocument doc;
		tinyxml2::XMLError ret = doc.LoadFile(path);
		if (ret != 0) {
			fprintf(stderr, "fail to load xml file: %s\n", path);
			return -1;
		}

		tinyxml2::XMLElement* root = doc.RootElement();
		fprintf(stdout, "root element name: %s\n", root->Name());

		// User
		tinyxml2::XMLElement* user = root->FirstChildElement("User");
		if (!user) {
			fprintf(stderr, "no child element: User\n");
			return -1;
		}
		fprintf(stdout, "user name: %s\n", user->Attribute("Name"));
	}


	int util::LoadScene(const char* path)
	{
		tinyxml2::XMLDocument doc;
		tinyxml2::XMLError ret = doc.LoadFile(path);
		if (ret != 0) {
			fprintf(stderr, "fail to load xml file: %s\n", path);
			return -1;
		}

		tinyxml2::XMLElement* root = doc.RootElement();
		fprintf(stdout, "root element name: %s\n", root->Name());

		XMLElement *surface = root->FirstChildElement("node");
		while (surface)
		{
			XMLElement *surfaceChild = surface->FirstChildElement();
			const char* content;
			const XMLAttribute *attributeOfSurface = surface->FirstAttribute();
			cout << attributeOfSurface->Name() << ":" << attributeOfSurface->Value() << endl;
			while (surfaceChild)
			{
				content = surfaceChild->GetText();
				surfaceChild = surfaceChild->NextSiblingElement();
				cout << content << endl;
			}
			surface = surface->NextSiblingElement();
		}
	}



	bool util::CircleAttack(float radius, vector3 attack, vector3 skill)
	{
		float distance = (attack - skill).length();
		return distance <= radius;
	}

	bool util::RectAttack(vector3 attacker, vector3 attacked, vector3 forward, float length, float width)
	{
		forward.normalize();
		auto deltaA = attacked - attacker;
		float forwardDotA = forward.dot(deltaA);
		if (forwardDotA > 0 && forwardDotA <= length)
		{

			auto right = forward.rotateY(-PI / 2);
			if (abs(forward.dot(deltaA)) < width)
			{
				return true;
			}
		}
		return false;
	}

	bool util::SectorAttack(vector3 attacker, vector3 attacked, vector3 forward, float angle, float raduis)
	{
		vector3 deltaA = attacked - attacker;
		deltaA.normalize();
		float tmpAngle = acos(deltaA.dot(forward)); //»¡¶È
		tmpAngle = tmpAngle * PI / 180; //½Ç¶È
		deltaA = attacked - attacker;
		if (tmpAngle < angle *0.5f && deltaA.length() < raduis)
		{
			return true;
		}
		return false;
	}

	vector3 util::Angle2Forward(float angle)
	{
		angle = angle * PI / 180;
		return vector3(sin(angle), 0, cos(angle));
	}

}