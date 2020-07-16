#include <iostream>
#include <memory>
#include "EntitySystem.hpp"
#include "../type.hpp"
#include "../log.hpp"
#include "../EngineInfo.hpp"
#include "../interface.hpp"
#include "../Util/util.hpp"
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

		Path* path;
		PathSystem::Instance()->Get("man.xml", path);
		player->Add<Rotation>(2);
		player->Add<Position>(p);
		unsigned int uid = util::GetIncUID();
		int confid = 1001;
		player->Add<Role>(uid, confid, 2, 1, 2, 3, path);
		player->OnEntityReleased += OnEntityDestroy;
		player->Add<Skill>("sk1002.xml");
		if (roleDelegate) roleDelegate(uid, confid);

		auto monster = _pool->CreateEntity();
		monster->Add<Rotation>(90);
		p.z = 45;
		monster->Add<Position>(p);
		PathSystem::Instance()->Get("monster.xml", path);
		uid = util::GetIncUID();
		confid = 1003;
		monster->Add<Monster>(uid, confid, 2, 1, 2, 4, path);
		monster->OnEntityReleased += OnEntityDestroy;
		monster->Add<Skill>("sk1001.xml");
		if (roleDelegate) roleDelegate(uid, confid);
	}

	void EntitySystem::Execute() {
		Hit();
	}


	bool EntitySystem::Hit()
	{
		float time = EngineInfo::time;

		for (auto &e : _group.lock()->GetEntities()) {
			auto pos = e->Get<Position>();
			auto rot = e->Get<Rotation>();
			if (e->Has<Skill>())
			{
				auto skill = e->Get<Skill>();
				size_t idx = skill->Find(time);
				size_t invalid = -1;
				if (idx != invalid)
				{
					Caster(idx, skill, pos->pos, rot->v);
				}
				else
				{
					ERR("skill index is invalid");
				}
			}
			else
			{
				WARN("entity not has skill component");
			}
		}
		return false;
	}


	void  EntitySystem::Caster(size_t idx, Skill* skill, vector3 rolePos, float roleRot)
	{
		int shape = skill->shapes[idx];
		float arg = skill->arg[idx];
		float  arg2 = skill->arg2[idx];

		for (auto &e : _group2.lock()->GetEntities()) {
			auto pos = e->Get<Position>();
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
				WARN("error unknown shape: " );
			}
		}
	}

	void EntitySystem::CalHurt(size_t idx, Skill* skill, EntityPtr e)
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
				else
				{
					std::string str = "not handle attribute " + std::string(type);
					LOG(str);
				}
			}
		}
	}
}
