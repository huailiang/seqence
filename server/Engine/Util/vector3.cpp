#include <vector>
#include<iostream>
#include "vector3.hpp"

using namespace std;

namespace Entitas
{
	const double uZero = 1e-6;

	vector3::vector3(const vector3 &v) :x(v.x), y(v.y), z(v.z)
	{
	}

	vector3::~vector3()
	{
	}

	void vector3::operator=(const vector3 &v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	vector3 vector3::operator+(const vector3 &v)
	{
		return vector3(x + v.x, y + v.y, z + v.z);
	}

	vector3 vector3::operator-(const vector3 &v)
	{
		return vector3(x - v.x, y - v.y, z - v.z);
	}

	vector3 vector3::operator/(const vector3 &v)
	{
		if (fabsf(v.x) <= uZero || fabsf(v.y) <= uZero || fabsf(v.z) <= uZero)
		{
			std::cerr << "Over flow!\n";
			return *this;
		}
		return vector3(x / v.x, y / v.y, z / v.z);
	}

	vector3 vector3::operator*(const vector3 &v)
	{
		return vector3(x*v.x, y*v.y, z*v.z);
	}

	vector3 vector3::operator+(float f)
	{
		return vector3(x + f, y + f, z + f);
	}

	vector3 vector3::operator-(float f)
	{
		return vector3(x - f, y - f, z - f);
	}

	vector3 vector3::operator/(float f)
	{
		if (fabsf(f) < uZero)
		{
			std::cerr << "Over flow!\n";
			return *this;
		}
		return vector3(x / f, y / f, z / f);
	}

	vector3 vector3::operator*(float f)
	{
		return vector3(x*f, y*f, z*f);
	}

	float vector3::dot(const vector3 &v)
	{
		return x * v.x + y * v.y + z * v.z;
	}

	float vector3::length()
	{
		return sqrtf(dot(*this));
	}

	void vector3::normalize()
	{
		float len = length();
		if (len < uZero) len = 1;
		len = 1 / len;

		x *= len;
		y *= len;
		z *= len;
	}

	/*
	Cross Product²æ³Ë¹«Ê½
	aXb = | i,  j,  k  |
		 | a.x a.y a.z|
		 | b.x b.y b.z| = (a.y*b.z -a.z*b.y)i + (a.z*b.x - a.x*b.z)j + (a.x*b.y - a.y*b.x)k
	*/
	vector3 vector3::crossProduct(const vector3 &v)
	{
		return vector3(y * v.z - z * v.y,
			z * v.x - x * v.z,
			x * v.y - y * v.x);
	}

	void vector3::printVec3()
	{
		std::cout << "(" << x << ", " << y << ", " << z << ")" << std::endl;
	}
}