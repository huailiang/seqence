#ifndef __movesystem__
#define __movesystem__

#include <iostream>
#include "../Entitas/ISystem.hpp"
#include "../Entitas/Group.hpp"
#include "../Component/XActor.hpp"

namespace Entitas
{
	using namespace std;

	class MoveSystem : public IExecuteSystem, public ISetPoolSystem {

	public:
		void SetPool(Pool* pool);

		void Execute();

		void SyncMove(EntityPtr e, XActor* actor);

	private:
		Pool* _pool;
		std::weak_ptr<Group> _group, _group2;
	};
}

#endif
