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
			e->Add<Position>(1.0f, 2.0f, 3.0f);
			e->Add<Move>(2);
		}

		void Execute() {
			for (auto &e : _group.lock()->GetEntities()) {
				auto pos = e->Get<Position>();
				if (e->Has<Move>())
				{
					auto move = e->Get<Move>();
					e->Replace<Position>(pos->x, pos->y + move->speed, pos->z);
					cout << e->Get<Position>()->y << endl;
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