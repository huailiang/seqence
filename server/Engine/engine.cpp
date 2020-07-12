#include "engine.hpp"
#include "interface.hpp"
#include "EngineInfo.hpp"
#include "Entitas/Matcher.hpp"
#include "System/MoveSystem.hpp"
#include "System/DemoSystem.hpp"
#include "System/EntitySystem.hpp"
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
    
    SyncPos posDelegate;
    SyncRot rotDelegate;
    SyncPlay  playDelegate;
    BroadCast broadDelegate;
    
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
        
        float* time;
        size_t cnt =0;
        vector3* pos;
        float* rot;
        util::LoadPath("man.xml", cnt, time, pos, rot);
        printf("cnt: %ld, time0: %f, time1: %f rot: %f\n", cnt, time[0], time[1], rot[1]);
        pos[0].tostring();
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
