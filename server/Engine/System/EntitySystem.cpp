#include <iostream>
#include <memory>
#include "EntitySystem.hpp"
#include "../type.hpp"
#include "../EngineInfo.hpp"
#include "../interface.hpp"
#include "../Component/Path.hpp"
#include "../Conf/PathSystem.hpp"
#include "../Component/XRole.hpp"
#include "../Component/XMonster.hpp"


namespace Entitas
{

	void EntitySystem::OnEntityDestroy(Entity* entity)
	{
		// sync to server
		if (entity->Has<XActor>())
		{
			auto ator = entity->Get<XActor>();
			printf("sync to client: %d", ator->uid);
		}
	}

	void EntitySystem::SetPool(Pool* pool) {
		_group = pool->GetGroup(Matcher_AllOf(Role));
		_group2 = pool->GetGroup(Matcher_AllOf(Monster));
		_pool = pool;
	}

	void EntitySystem::Initialize() {
		auto player = _pool->CreateEntity();
		vector3 p;
		p.x = 1;

		auto path = PathSystem::Instance()->Get("man.xml");
		player->Add<Rotation>(2);
		player->Add<Position>(p);
		unsigned int uid = 1001;
		int confid = util::GetIncUID();
		player->Add<Role>(uid, confid, 2, 1, 2, 3, path);
		player->OnEntityReleased += OnEntityDestroy;
		roleDelegate(uid, confid);

		auto monster = _pool->CreateEntity();
		monster->Add<Rotation>(90);
		p.z = 45;
		monster->Add<Position>(p);
		path = PathSystem::Instance()->Get("monster.xml");
		uid = util::GetIncUID();
		confid = 1003;
		monster->Add<Monster>(uid, confid, 2, 1, 2, 4, path);
		monster->OnEntityReleased += OnEntityDestroy;
		roleDelegate(uid, confid);
	}

	void EntitySystem::Execute() {
		float time = EngineInfo::time;
		for (auto &e : _group.lock()->GetEntities()) {
			if (e->Has<XActor>())
			{
				auto actor = e->Get<XActor>();
				auto path = actor->path;
				float rot;
				auto pos = path->Sample(time, rot);
				if (e->Has<Position>())
				{
					auto c_pos = e->Get<Position>();
					c_pos->Reset(pos);
				}
				if (e->Has<Rotation>())
				{
					auto c_rot = e->Get<Rotation>();
					c_rot->Reset(rot);
				}
				posDelegate(actor->uid, pos.x, pos.y, pos.z, rot);
			}
			else
			{
				printf("no postion exist");
			}
		}
		Hit();
	}


	bool EntitySystem::Hit()
	{
		float time = EngineInfo::time;

		for (auto &e : _group.lock()->GetEntities()) {
			auto pos = e->Get<Position>();
			auto rot = e->Get<Rotation>();
			auto skill = e->Get<Skill>();
			if (skill)
			{
				int idx = skill->Find(time);
				if (idx >= 0)
				{
					Caster(idx, skill, pos->pos, rot->v);
				}
			}
		}
		return false;
	}


	void  EntitySystem::Caster(int idx, Skill* skill, vector3 rolePos, float roleRot)
	{
		int shape = skill->shapes[idx];
		float arg = skill->arg[idx];
		float  arg2 = skill->arg2[idx];

		for (auto &e : _group2.lock()->GetEntities()) {
			auto pos = e->Get<Position>();
			auto rot = e->Get<Rotation>();
			if (shape == RING)
			{
				if (util::CircleAttack(arg, rolePos, pos->pos))
				{
					CalHurt(idx, skill, e);
				}
			}
			else if (shape == SECTOR)
			{
				vector3 forward = util::Angle2Forward(roleRot);
				if (util::SectorAttack(rolePos, pos->pos, forward, arg2, arg))
				{
					CalHurt(idx, skill, e);
				}
			}
			else if (shape == RECT)
			{
				vector3 forward = util::Angle2Forward(roleRot);
				if (util::RectAttack(rolePos, pos->pos, forward, arg, arg2))
				{
					CalHurt(idx, skill, e);
				}
			}
			else
			{
				printf("error unknown shape: %d", shape);
			}
		}
	}

	void EntitySystem::CalHurt(int idx, Skill* skill, EntityPtr e)
	{
		std::vector<const char*> types = skill->types[idx];
		std::vector<float> effects = skill->effect[idx];
		auto size = types.size();
		if (e->Has<XActor>())
		{
			auto ator = e->Get<XActor>();
			for (size_t i = 0; i < size; i++)
			{
				const char* type = types[i];
				float effect = effects[i];
				if (strcmp(type, "HP") == 0)
				{
					ator->hp -= effect;
				}
				else if (strcmp(type, "SP") == 0)
				{
					ator->sp -= effect;
				}
			}
		}
	}

}