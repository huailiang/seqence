#include <vector>
#include <iostream>
#include <math.h>
#include "util.hpp"
#include "../EngineInfo.hpp"
#include "tinyxml2.h"


using namespace std;
using namespace tinyxml2;

namespace Entitas
{

	const float PI = 3.1415926;
    
    string util::GetAssetPath(const char* name)
    {
        const char* dir=  EngineInfo::assetPath;
        string s1(dir);
        string s2(name);
        return s1+s2;
    }
    
    int util::LoadPath(const char* name, size_t& cnt, float*& time, vector3*& pos, float*& rot)
    {
        tinyxml2::XMLDocument doc;
        string path = GetAssetPath(name);
        tinyxml2::XMLError ret = doc.LoadFile(path.c_str());
        if (ret != 0) {
            fprintf(stderr, "fail to load xml2 file: %s\n", name);
            return -1;
        }
        tinyxml2::XMLElement* root = doc.RootElement();
        XMLElement *t = root->FirstChildElement("time");
        XMLElement *v= t->FirstChildElement("float");
        
        cnt =0;
        while (v) {
            v=v->NextSiblingElement();
            cnt++;
        }
        
        time = new float[cnt];
        pos = new vector3[cnt];
        rot = new float[cnt];
        int i=0;
        v= t->FirstChildElement("float");
        while (v) {
            float vl  =atof(v->GetText());
             printf("v: %f\n", vl);
            time[i++] = vl;
             v= v->NextSiblingElement();
        }
        t = root->FirstChildElement("pos");
        v= t->FirstChildElement("Vector4");
        i=0;
        while (v) {
            auto x= atof(v->FirstChildElement("x")->GetText());
            auto y = atof(v->FirstChildElement("y")->GetText());
            auto z = atof(v->FirstChildElement("z")->GetText());
            auto w = atof(v->FirstChildElement("w")->GetText());
            vector3 vec(x,y,z);
            pos[i]= vec;
            rot[i++]=w;
            v=v->NextSiblingElement();
        }
        doc.Clear();
        return 0;
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
