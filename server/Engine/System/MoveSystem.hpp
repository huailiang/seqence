#ifndef __movesystem__
#define __movesystem__

#include "../Component/Move.hpp"
#include "../Component/Position.hpp"
#include "../Entitas/ISystem.hpp"
#include <iostream>


namespace Entitas
{
	using namespace std;

	class MoveSystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(Position));
			_pool = pool;
		}

		void Initialize() {
			auto e = _pool->CreateEntity();
			vector3 p;
			p.x = 1;
			p.y = 2;
			e->Add<Position>(p);
			e->Add<Move>(2);
		}

		void Execute() {
			for (auto &e : _group.lock()->GetEntities()) {
				auto pos = e->Get<Position>();
				if (e->Has<Move>())
				{
					auto move = e->Get<Move>();
					pos->pos.y += move->speed;
					e->Replace<Position>(pos->pos);
                    cout <<"pos Y: "<< e->Get<Position>()->pos.y << endl;
				}
				else
				{
					cout << "no move exist" << endl;
				}
			}
		}

	private:
		Pool* _pool;
		std::weak_ptr<Group> _group;
	};

}


#endif
