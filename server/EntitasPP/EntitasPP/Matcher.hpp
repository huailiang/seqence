#pragma once

#include "Entity.hpp"

namespace EntitasPP
{
	class Matcher;
	class TriggerOnEvent;
	typedef std::vector<Matcher> MatcherList;

	class Matcher
	{
	public:
		Matcher() = default;
		static auto AllOf(const ComponentIdList indices) -> const Matcher;
		static auto AllOf(const MatcherList matchers) -> const Matcher;
		static auto AnyOf(const ComponentIdList indices) -> const Matcher;
		static auto AnyOf(const MatcherList matchers) -> const Matcher;
		static auto NoneOf(const ComponentIdList indices) -> const Matcher;
		static auto NoneOf(const MatcherList matchers) -> const Matcher;

		bool IsEmpty() const;
		bool Matches(const EntityPtr& entity);
		auto GetIndices() -> const ComponentIdList;
		auto GetAllOfIndices() const -> const ComponentIdList;
		auto GetAnyOfIndices() const -> const ComponentIdList;
		auto GetNoneOfIndices() const -> const ComponentIdList;

		auto GetHashCode() const -> unsigned int;
		bool CompareIndices(const Matcher& matcher) const;

		auto OnEntityAdded() -> const TriggerOnEvent*;
		auto OnEntityRemoved() -> const TriggerOnEvent*;
		auto OnEntityAddedOrRemoved() -> const TriggerOnEvent*;

		bool operator ==(const Matcher right) const;

	protected:
		void CalculateHash();

		ComponentIdList mIndices;
		ComponentIdList mAllOfIndices;
		ComponentIdList mAnyOfIndices;
		ComponentIdList mNoneOfIndices;

	private:
		auto ApplyHash(unsigned int hash, const ComponentIdList indices, int i1, int i2) const -> unsigned int;
		auto MergeIndices() const->ComponentIdList;
		static auto MergeIndices(MatcherList matchers)->ComponentIdList;
		static auto DistinctIndices(ComponentIdList indices)->ComponentIdList;

		unsigned int mCachedHash{ 0 };
	};
}

namespace std
{
	template <>
	struct hash<EntitasPP::Matcher>
	{
		std::size_t operator()(const EntitasPP::Matcher& matcher) const
		{
			return hash<unsigned int>()(matcher.GetHashCode());
		}
	};
}

namespace
{
#define FUNC_1(MODIFIER, X) MODIFIER(X)
#define FUNC_2(MODIFIER, X, ...) MODIFIER(X), FUNC_1(MODIFIER, __VA_ARGS__)
#define FUNC_3(MODIFIER, X, ...) MODIFIER(X), FUNC_2(MODIFIER, __VA_ARGS__)
#define FUNC_4(MODIFIER, X, ...) MODIFIER(X), FUNC_3(MODIFIER, __VA_ARGS__)
#define FUNC_5(MODIFIER, X, ...) MODIFIER(X), FUNC_4(MODIFIER, __VA_ARGS__)
#define FUNC_6(MODIFIER, X, ...) MODIFIER(X), FUNC_5(MODIFIER, __VA_ARGS__)
#define GET_MACRO(_1, _2, _3, _4, _5, _6, NAME,...) NAME
#define FOR_EACH(MODIFIER,...) GET_MACRO(__VA_ARGS__, FUNC_6, FUNC_5, FUNC_4, FUNC_3, FUNC_2, FUNC_1)(MODIFIER, __VA_ARGS__)

#define COMPONENT_GET_TYPE_ID(COMPONENT_CLASS) EntitasPP::ComponentTypeId::Get<COMPONENT_CLASS>()
#define Matcher_AllOf(...) ((EntitasPP::Matcher)EntitasPP::Matcher::AllOf(std::vector<EntitasPP::ComponentId>({ FOR_EACH(COMPONENT_GET_TYPE_ID, __VA_ARGS__) })))
#define Matcher_AnyOf(...) ((EntitasPP::Matcher)EntitasPP::Matcher::AnyOf(std::vector<EntitasPP::ComponentId>({ FOR_EACH(COMPONENT_GET_TYPE_ID, __VA_ARGS__) })))
#define Matcher_NoneOf(...) ((EntitasPP::Matcher)EntitasPP::Matcher::NoneOf(std::vector<EntitasPP::ComponentId>({ FOR_EACH(COMPONENT_GET_TYPE_ID, __VA_ARGS__) })))
}