#include "../Component/DemoComponent.hpp"
#include "../Entitas/ISystem.hpp"
#include <iostream>


namespace Entitas
{

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
			std::cout << "There are " << entitiesCount << " entities with the DemoComponent" << std::endl;
			std::cout << "DemoSystem executed" << std::endl;
		}

	private:
		Pool* _pool;
	};

}