#ifndef  __entitysystem__
#define __entitysystem__

#include <iostream>
#include "../Component/Skill.hpp"
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

		void Caster(int idx, Skill* skill, vector3 rolePos, float roleRot);

		void CalHurt(int idx, Skill* skill, EntityPtr ator);

		static void OnEntityDestroy(Entity* entity);


	private:
		Pool* _pool;
		std::weak_ptr<Group> _group, _group2;
	};

}

#endif
