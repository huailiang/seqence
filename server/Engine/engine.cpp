#include "engine.hpp"
#include "Entitas/Matcher.hpp"
#include "System/MoveSystem.hpp"
#include "System/DemoSystem.hpp"
#include "Util/util.hpp"

namespace Entitas
{

	SystemContainer *systems;
	Pool* pool;


	void Initial()
	{
		printf("engine initial");
		systems = new SystemContainer();
		pool = new Pool();

		systems->Add(pool->CreateSystem<DemoSystem>());
		systems->Add(pool->CreateSystem<MoveSystem>());
		systems->Initialize();
		util::LoadPath("../Engine/assets/tinyxml.xml");
		util::LoadScene("../Engine/assets/scene.xml");
	}


	void Update(float delta)
	{
		//printf("engine update %.3f \n", delta);
		if (systems)
		{
			//systems->Execute();
		}
	}

	void Destroy()
	{
		delete systems;
		delete pool;
		printf("engine quit");
	}

}