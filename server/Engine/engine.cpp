#include "engine.hpp"
#include "EngineInfo.hpp"
#include "Entitas/Matcher.hpp"
#include "System/MoveSystem.hpp"
#include "System/DemoSystem.hpp"
#include "Util/util.hpp"

namespace Entitas
{
	SystemContainer *systems;
	Pool* pool;
    
    int EngineInfo::frameRate;
    int EngineInfo::frameCount;
    float EngineInfo::time;
    float EngineInfo::delta;

	void Initial(int rate)
	{
		printf("engine initial");
		EngineInfo::frameRate = 30;
		EngineInfo::frameCount = 0;
		EngineInfo::time = 0;

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
			systems->Execute();
		}
		EngineInfo::frameCount++;
		EngineInfo::delta = delta;
		EngineInfo::time += delta;
	}

	void Destroy()
	{
		delete systems;
		delete pool;
		printf("engine quit");
	}

}
