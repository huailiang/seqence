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
            _pool->CreateEntity()->Add<DemoComponent>("foo2", "bar");
            auto entitiesCount = _pool->GetGroup(Matcher_AllOf(DemoComponent))->Count();
            std::cout << "DemoSystem initialized: "<<entitiesCount << std::endl;
		}
		void Execute() {
			auto entitiesCount = _pool->GetGroup(Matcher_AllOf(DemoComponent))->Count();
			std::cout << "There are " << entitiesCount << " entities with the DemoComponent" << std::endl;
		}

	private:
		Pool* _pool;
	};

}
