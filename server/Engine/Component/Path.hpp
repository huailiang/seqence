#ifndef  __path__
#define __path__

#include "../Util/vector3.hpp"
#include "../Util/util.hpp"

namespace Entitas
{

	class Path : public IComponent {
        
	public:
		void Reset(const char* name) {
            util::LoadPath(name, cnt, time, pos, rot);
		}
        
        vector3 Sample(float t, float*& r)
        {
            if (t<time[0]) {
                r = &rot[0];
                return pos[0];
            }
            else if(t>time[cnt-1])
            {
                r = &rot[cnt-1];
                return pos[cnt-1];
            }
            else
            {
                for(int i=0;i<cnt-1;i++)
                {
                    if(time[i]<t && time[i+1]>t)
                    {
                        float p = (time[i+1]-t)/(time[i+1]-time[i]);
                        float y = util::lerp(rot[i], rot[i+1], p);
                        r = &y;
                        return util::lerp(pos[i], pos[i+1], p);
                    }
                }
            }
            return vector3(0,0,0);
        }

		~Path()
		{
			delete[] time;
			delete[] pos;
			delete[] rot;
			cnt = 0;
		}

    private:
        size_t cnt;
        float* time;
		vector3* pos;
        float* rot;
	};
}

#endif
