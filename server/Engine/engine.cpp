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
    const char* EngineInfo::assetPath;

    /*
     * 第一个参数是fps
     * 第二个参数是asset目录（资源目录）
     */
	void Initial(int rate, const char* assets)
	{
		printf("engine initial");
		EngineInfo::frameRate = 30;
		EngineInfo::frameCount = 0;
		EngineInfo::time = 0;
        EngineInfo::assetPath = assets;

		systems = new SystemContainer();
		pool = new Pool();

		systems->Add(pool->CreateSystem<DemoSystem>());
		systems->Add(pool->CreateSystem<MoveSystem>());
		systems->Initialize();
		util::LoadPath("tinyxml.xml");
		util::LoadScene("scene.xml");
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
