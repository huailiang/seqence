#include <iostream>
#include "../Component/Attribute.hpp"
#include "../Component/Position.hpp"
#include "../Entitas/ISystem.hpp"


namespace Entitas
{
	using namespace std;

	class EntitySystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

	public:
		void SetPool(Pool* pool) {
			_group = pool->GetGroup(Matcher_AllOf(Attribute));
			_pool = pool;
		}

		void Initialize() {
			auto e = _pool->CreateEntity();
			e->Add<Position>(1.0f, 2.0f, 3.0f);
			e->Add<Attribute>(2, 1, 2, 3);
		}

		void Execute() {
			for (auto &e : _group.lock()->GetEntities()) {
				auto attr = e->Get<Attribute>();
				if (e->Has<Position>())
				{
					auto pos = e->Get<Position>();
					e->Replace<Position>(pos->x, pos->y, pos->z);
					cout << e->Get<Position>()->y << endl;
				}
				else
				{
					cout << "no postion exist" << endl;
				}
			}
		}
	private:
		Pool* _pool;
		std::weak_ptr<Group> _group;
	};

}