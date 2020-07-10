#ifndef  __vector3__
#define __vector3__


namespace Entitas
{

	class vector3
	{
	public:
		vector3() :x(0), y(0), z(0) {}
		vector3(float x1, float y1, float z1) :x(x1), y(y1), z(z1) {}
		vector3(const vector3 &v);
		~vector3();

		void operator=(const vector3 &v);
		vector3 operator+(const vector3 &v);
		vector3 operator-(const vector3 &v);
		vector3 operator/(const vector3 &v);
		vector3 operator*(const vector3 &v);
		vector3 operator+(float f);
		vector3 operator-(float f);
		vector3 operator/(float f);
		vector3 operator*(float f);
		
		float dot(const vector3 &v);
		float length();
		void normalize();
		vector3 cross(const vector3 &v);
		void tostring();

		vector3 rotateY(const float v);

	public:
		float x, y, z;
	};
}

#endif // ! __vector3__