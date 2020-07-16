#ifndef  __entitysystem__
#define __entitysystem__

#include <iostream>
#include "../Component/Attack.hpp"
#include "../Component/Rotation.hpp"
#include "../Component/Position.hpp"
#include "../Entitas/ISystem.hpp"


namespace Entitas
{

	class EntitySystem : public IInitializeSystem, public IExecuteSystem, public ISetPoolSystem {

        
	public:

		void SetPool(Pool* pool);

		void Initialize();
        
		void Execute();

		bool Hit();

		void Caster(size_t idx, Attack* attack, vector3 rolePos, float roleRot);

		void CalHurt(size_t idx, Attack* attack, EntityPtr ator);

		static void OnEntityDestroy(Entity* entity);


	private:
		Pool* _pool;
		std::weak_ptr<Group> _group, _group2;
	};

}

#endif
