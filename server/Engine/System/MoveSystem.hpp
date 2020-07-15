#ifndef __movesystem__
#define __movesystem__

#include <iostream>
#include "../Component/Move.hpp"
#include "../Component/Position.hpp"
#include "../Component/Rotation.hpp"
#include "../Component/XRole.hpp"
#include "../Entitas/ISystem.hpp"


namespace Entitas
{
	using namespace std;

	class MoveSystem :  public IExecuteSystem, public ISetPoolSystem {

	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(XActor));
			_pool = pool;
		}


		void Execute() {
            float time = EngineInfo::time;
			for (auto &e : _group.lock()->GetEntities()) {
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
                posDelegate(actor->uid, pos.x, pos.y, pos.z, rot);			}
		}

	private:
		Pool* _pool;
		std::weak_ptr<Group> _group;
	};

}


#endif
