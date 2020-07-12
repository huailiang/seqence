#include <iostream>
#include "../Component/Attribute.hpp"
#include "../Component/Rotation.hpp"
#include "../Entitas/ISystem.hpp"
#include "../Component/Path.hpp"

namespace Entitas
{

	class EntitySystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

        
	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(Attribute));
			_pool = pool;
		}

		void Initialize() {
            
		}
        
        void CreateEntity()
        {
            auto player = _pool->CreateEntity();
            vector3 p;
            p.x = 1;
            player->Add<Rotation>(2);
            player->Add<Position>(p);
            player->Add<Attribute>(2, 1, 2, 3);
            player->Add<Path>("man.xml");
            player->OnEntityReleased += OnEntityDestroy;
        }

		void Execute() {
			for (auto &e : _group.lock()->GetEntities()) {
				auto attr = e->Get<Attribute>();
				if (e->Has<Position>() && e->Has<Rotation>())
				{
					auto pos = e->Get<Position>();
                    auto rot = e->Get<Rotation>();
                    pos->pos.y+=2;
					e->Replace<Position>(pos->pos);
                    posDelegate(1,pos->pos.x,pos->pos.y,pos->pos.z);
                    Hit(e,pos->pos,rot->v);
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
        
        bool Hit(EntityPtr e, vector3 p, float rot)
        {
            float time = EngineInfo::time;
            return false;
        }


	private:
		Pool* _pool;
		std::weak_ptr<Group> _group;
	};

}
