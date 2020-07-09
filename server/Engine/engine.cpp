#include "Entitas/Matcher.hpp"
#include "Entitas/Pool.hpp"
#include "engine.hpp"
#include <iostream>
#include <string>

using namespace std;


namespace Entitas
{


	class DemoComponent : public IComponent {
	public:
		void Reset(const std::string& name1, const std::string& name2) {
			std::cout << "Created new entity: " << name1 << "," << name2 << std::endl;
		}
	};

	class Position : public IComponent {
	public:
		void Reset(float px, float py, float pz) {
			x = px;
			y = py;
			z = pz;
		}

		float x;
		float y;
		float z;
	};


	class Move : public IComponent
	{
	public:
		void Reset(float _speed) {
			speed = _speed;
		}

		float speed;
	};


	class DemoSystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {
	public:
		void SetPool(Pool* pool) {
			_pool = pool;
		}
		void Initialize() {
			_pool->CreateEntity()->Add<DemoComponent>("foo", "bar");
			std::cout << "DemoSystem initialized" << std::endl;
		}
		void Execute() {
			_pool->CreateEntity()->Add<DemoComponent>("foo", "bar");
			auto entitiesCount = _pool->GetGroup(Matcher_AllOf(DemoComponent))->Count();
			std::cout << "There are " << entitiesCount << " entities with the component DemoComponent" << std::endl;
			std::cout << "DemoSystem executed" << std::endl;
		}

	private:
		Pool* _pool;
	};

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


	SystemContainer *systems;
	Pool* pool;


	void Initial()
	{
		cout << "engine initial" << endl;
		systems = new SystemContainer();
		pool = new Pool();

		systems->Add(pool->CreateSystem<DemoSystem>());
		systems->Add(pool->CreateSystem<MoveSystem>());
		systems->Initialize();
	}


	void Update(float delta)
	{
		//printf("engine update %.3f \n", delta);
		if (systems)
		{
			systems->Execute();
		}
	}

	void Destroy()
	{
		delete systems;
		delete pool;
		printf("engine quit");
	}

}