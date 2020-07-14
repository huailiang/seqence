#ifndef  __entitysystem__
#define __entitysystem__

#include <iostream>
#include "../Component/XRole.hpp"
#include "../Component/XMonster.hpp"
#include "../Component/Rotation.hpp"
#include "../Entitas/ISystem.hpp"
#include "../Component/Path.hpp"

namespace Entitas
{

	class EntitySystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

        
	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(Role));
			_pool = pool;
		}

		void Initialize() {
			auto player = _pool->CreateEntity();
			vector3 p;
			p.x = 1;
			player->Add<Rotation>(2);
			player->Add<Position>(p);
			player->Add<Role>(1001, 2, 1, 2, 3);
			player->Add<Path>("man.xml");
			player->OnEntityReleased += OnEntityDestroy;

			auto monster = _pool->CreateEntity();
			monster->Add<Rotation>(90);
			p.z = 45;
			monster->Add<Position>(p);
			monster->Add<Path>("monster.xml");
			monster->Add<Monster>(1002, 2, 1, 2, 4);
			monster->OnEntityReleased += OnEntityDestroy;
		}
        

		void Execute() {
			for (auto &e : _group.lock()->GetEntities()) {
				auto attr = e->Get<Role>();
				if (e->Has<Position>() && e->Has<Rotation>())
				{
					auto pos = e->Get<Position>();
					auto rot = e->Get<Rotation>();
					pos->pos.y += 2;
					e->Replace<Position>(pos->pos);
					posDelegate(attr->uid, pos->pos.x, pos->pos.y, pos->pos.z, rot->v);
					Hit(e);
				}
				else
				{
					printf("no postion exist");
				}
			}
		}

		static void OnEntityDestroy(Entity* entity)
		{
			// sync to server
            
		}

		void UpdatePosition()
		{

		}
        
        bool Hit(EntityPtr e)
        {
            float time = EngineInfo::time;
            auto path = e->Get<Path>();
            float* rot;
            auto pos = path->Sample(time, rot);
            return false;
        }


	private:
		Pool* _pool;
		std::weak_ptr<Group> _group;
	};

}

#endif
