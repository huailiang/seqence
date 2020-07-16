#include "MoveSystem.hpp"
#include "../EngineInfo.hpp"
#include "../interface.hpp"
#include "../Component/Position.hpp"
#include "../Component/Rotation.hpp"
#include "../Component/XRole.hpp"
#include "../Component/XMonster.hpp"


namespace Entitas
{
	using namespace std;


	void MoveSystem::SetPool(Pool* pool) 
	{
		_group = pool->GetGroup(Matcher_AllOf(XRole));
		_group2 = pool->GetGroup(Matcher_AllOf(XMonster));
		_pool = pool;
	}


	void MoveSystem::Execute()
	{
		for (auto &e : _group.lock()->GetEntities())
		{
			auto role = e->Get<XRole>();
			XActor* actor = static_cast<XActor*>(role);
			SyncMove(e, actor);
		}
		for (auto &e : _group2.lock()->GetEntities())
		{
			auto monster = e->Get<XMonster>();
			XActor* actor = static_cast<XActor*>(monster);
			SyncMove(e, actor);
		}
	}


	void MoveSystem::SyncMove(EntityPtr e, XActor* actor)
	{
		float time = EngineInfo::time;
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
		if (posDelegate)
		{
			posDelegate(actor->uid, pos.x, pos.y, pos.z, rot);
		}
		else
		{
			printf("uid: %d pos: (%f,%f,%f) \n", actor->uid, pos.x, pos.y, pos.z);
		}
	}

}