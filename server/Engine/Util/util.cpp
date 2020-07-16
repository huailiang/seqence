#include <vector>
#include <iostream>
#include <math.h>
#include "util.hpp"
#include "../EngineInfo.hpp"


using namespace std;
using namespace tinyxml2;

namespace Entitas
{

	const float PI = 3.1415926f;

	const char* EngineInfo::assetPath;

	unsigned int util::inc_uid;

	util::util()
	{
		inc_uid = 1 << 4;
	}

	string util::GetAssetPath(const char* name)
	{
		const char* dir = Entitas::EngineInfo::assetPath;
		string s1(dir);
		string s2(name);
		return s1 + s2 + ".xml";
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
		XMLElement *v = t->FirstChildElement("float");

		cnt = 0;
		while (v) {
			v = v->NextSiblingElement();
			cnt++;
		}

		time = new float[cnt];
		pos = new vector3[cnt];
		rot = new float[cnt];
		int i = 0;
		v = t->FirstChildElement("float");
		while (v) {
			float vl = v->FloatText(0);
			printf("v: %f\n", vl);
			time[i++] = vl;
			v = v->NextSiblingElement();
		}
		t = root->FirstChildElement("pos");
		v = t->FirstChildElement("Vector4");
		i = 0;
		while (v) {
			auto x = v->FirstChildElement("x")->FloatText(0);
			auto y = v->FirstChildElement("y")->FloatText(0);
			auto z = v->FirstChildElement("z")->FloatText(0);
			auto w = v->FirstChildElement("w")->FloatText(0);
			vector3 vec(x, y, z);
			pos[i] = vec;
			rot[i++] = w;
			v = v->NextSiblingElement();
		}
		return 0;
	}

	int util::LoadSkill(const char* skill, size_t& cnt, float*& start, float*& duration, int*& shapes,
		float*& arg, float*& arg2, std::vector<const char*>*& types, std::vector<float>*& effect)
	{
		tinyxml2::XMLDocument doc;
		string path = GetAssetPath(skill);
		tinyxml2::XMLError ret = doc.LoadFile(path.c_str());
		if (ret != 0) {
			fprintf(stderr, "fail to load skill : %s\n", skill);
			return -1;
		}
		tinyxml2::XMLElement* root = doc.RootElement();
		auto host = root->FirstChildElement("skillHostTrack");
		int hostIdx = host->IntText(0);
		XMLElement *tracks = root->FirstChildElement("tracks");

		size_t i = 0;
		cnt = 0;
		XMLElement *trackData = tracks->FirstChildElement("TrackData");
		while (trackData)
		{
			auto attr = trackData->Attribute("xsi:type");
			printf("track type: %s \n", attr);
			if (i++ == hostIdx)
			{
				auto childs = trackData->FirstChildElement("childs");
				if (childs)
				{
					auto track = childs->FirstChildElement("TrackData");
					while (track)
					{
						auto clips = track->FirstChildElement("clips");
						if (clips)
						{
							auto data = clips->FirstChildElement("ClipData");
							auto type = data->Attribute("xsi:type");
							if (strcmp(type, "LogicClipData") != 0)
							{
								track = track->NextSiblingElement();
								continue;
							}
							while (data)
							{
								data = data->NextSiblingElement("ClipData");
								cnt++;
							}
							start = new float[cnt];
							duration = new float[cnt];
							shapes = new int[cnt];
							arg = new float[cnt];
							arg2 = new float[cnt];
							types = new vector<const char*>[cnt];
							effect = new vector<float>[cnt];
							data = clips->FirstChildElement("ClipData");
							i = 0;
							while (i < cnt)
							{
								start[i] = data->FirstChildElement("start")->FloatText(0);
								duration[i] = data->FirstChildElement("duration")->FloatText(0);
								shapes[i] = data->FirstChildElement("attackShape")->IntText(0);
								arg[i] = data->FirstChildElement("attackArg")->FloatText(0);
								arg2[i] = data->FirstChildElement("attackArg2")->FloatText(0);
								auto xmltype = data->FirstChildElement("logicType");
								auto ltype = xmltype->FirstChildElement("LogicType");
								while (ltype)
								{
									types[i].push_back(ltype->GetText());
									ltype = ltype->NextSiblingElement();
								}
								auto xeffect = data->FirstChildElement("effect");
								auto effVal = xeffect->FirstChildElement("float");
								while (effVal)
								{
									effect[i].push_back(effVal->FloatText(0));
									effVal = effVal->NextSiblingElement();
								}
								data = data->NextSiblingElement();
								i++;
							}
						}
						track = track->NextSiblingElement();
					}
				}
				break;
			}
			trackData = trackData->NextSiblingElement();
		}
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

	unsigned int util::GetIncUID()
	{
		return inc_uid++;
	}

}
