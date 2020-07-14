#ifndef  __entitysystem__
#define __entitysystem__

#include <iostream>
#include "../Component/XRole.hpp"
#include "../Component/XMonster.hpp"
#include "../Component/Rotation.hpp"
#include "../Entitas/ISystem.hpp"
#include "../Component/Path.hpp"
#include "../Conf/PathSystem.hpp"

namespace Entitas
{

	class EntitySystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

    
        
	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(Role));
            _group2 =pool->GetGroup(Matcher_AllOf(Monster));
			_pool = pool;
		}

		void Initialize() {
			auto player = _pool->CreateEntity();
			vector3 p;
			p.x = 1;
            
            auto path = PathSystem::Instance()->Get("man.xml");
			player->Add<Rotation>(2);
			player->Add<Position>(p);
			player->Add<Role>(1001, 2, 1, 2, 3, path);
			player->OnEntityReleased += OnEntityDestroy;
            roleDelegate(1001, 1001);

			auto monster = _pool->CreateEntity();
			monster->Add<Rotation>(90);
			p.z = 45;
			monster->Add<Position>(p);
            path = PathSystem::Instance()->Get("monster.xml");
			monster->Add<Monster>(1003, 2, 1, 2, 4,path);
			monster->OnEntityReleased += OnEntityDestroy;
            roleDelegate(1003, 1003);
		}
        

		void Execute() {
            float time = EngineInfo::time;
			for (auto &e : _group.lock()->GetEntities()) {
				if (e->Has<XActor>())
				{
                    auto actor = e->Get<XActor>();
                    auto path = actor->path;
                    float* rot;
                    auto pos = path->Sample(time, rot);
                    if(e->Has<Position>())
                    {
                        auto c_pos = e->Get<Position>();
                        c_pos->Reset(pos);
                    }
                    if(e->Has<Rotation>())
                    {
                        auto c_rot = e->Get<Rotation>();
                        c_rot->Reset(*rot);
                    }
					posDelegate(actor->uid, pos.x, pos.y, pos.z, *rot);
                    delete rot;
				}
				else
				{
					printf("no postion exist");
				}
			}
            Hit();
		}

		static void OnEntityDestroy(Entity* entity)
		{
			// sync to server
            if(entity->Has<XActor>())
            {
                auto ator = entity->Get<XActor>();
                printf("sync to client: %d", ator->uid);
            }
		}
        
        bool Hit()
        {
            float time = EngineInfo::time;
            auto size = _group2.lock()->Count();
            vector3 arr_pos[size];
            float arr_rot[size];
            int i =0;
            for (auto &e : _group2.lock()->GetEntities()) {
                auto path = e->Get<Path>();
                float* rot;
                auto pos = path->Sample(time, rot);
                arr_pos[i] = pos;
                arr_rot[i] = *rot;
                delete rot;
            }

            for (auto &e : _group.lock()->GetEntities()) {
                auto path = e->Get<Path>();
                float* rot;
                auto pos = path->Sample(time, rot);
                for(int i=0;i<size;i++)
                {
                    
                }
            }
            return false;
        }


	private:
		Pool* _pool;
		std::weak_ptr<Group> _group, _group2;
	};

}

#endif
