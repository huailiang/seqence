#include "Matcher.hpp"
#include "TriggerOnEvent.hpp"
#include <algorithm>

namespace Entitas
{
	auto Matcher::AllOf(const ComponentIdList indices) -> const Matcher
	{
		auto matcher = Matcher();
		matcher.mAllOfIndices = DistinctIndices(indices);
		matcher.CalculateHash();
		return matcher;
	}

	auto Matcher::AllOf(const MatcherList matchers) -> const Matcher
	{
		return Matcher::AllOf(MergeIndices(matchers));
	}

	auto Matcher::AnyOf(const ComponentIdList indices) -> const Matcher
	{
		auto matcher = Matcher();
		matcher.mAnyOfIndices = DistinctIndices(indices);
		matcher.CalculateHash();
		return matcher;
	}

	auto Matcher::AnyOf(const MatcherList matchers) -> const Matcher
	{
		return Matcher::AnyOf(MergeIndices(matchers));
	}

	auto Matcher::NoneOf(const ComponentIdList indices) -> const Matcher
	{
		auto matcher = Matcher();
		matcher.mNoneOfIndices = DistinctIndices(indices);
		matcher.CalculateHash();
		return matcher;
	}

	auto Matcher::NoneOf(const MatcherList matchers) -> const Matcher
	{
		return Matcher::NoneOf(MergeIndices(matchers));
	}

	bool Matcher::IsEmpty() const
	{
		return (mAllOfIndices.empty() && mAnyOfIndices.empty() && mNoneOfIndices.empty());
	}

	bool Matcher::Matches(const EntityPtr& entity)
	{
		auto matchesAllOf = mAllOfIndices.empty() || entity->HasComponents(mAllOfIndices);
		auto matchesAnyOf = mAnyOfIndices.empty() || entity->HasAnyComponent(mAnyOfIndices);
		auto matchesNoneOf = mNoneOfIndices.empty() || !entity->HasAnyComponent(mNoneOfIndices);

		return matchesAllOf && matchesAnyOf && matchesNoneOf;
	}

	auto Matcher::GetIndices() -> const ComponentIdList
	{
		if (mIndices.empty())
		{
			mIndices = MergeIndices();
		}
		return mIndices;
	}

	auto Matcher::GetAllOfIndices() const -> const ComponentIdList
	{
		return mAllOfIndices;
	}

	auto Matcher::GetAnyOfIndices() const -> const ComponentIdList
	{
		return mAnyOfIndices;
	}

	auto Matcher::GetNoneOfIndices() const -> const ComponentIdList
	{
		return mNoneOfIndices;
	}

	auto Matcher::GetHashCode() const -> unsigned int
	{
		return mCachedHash;
	}

	bool Matcher::CompareIndices(const Matcher& matcher) const
	{
		if (matcher.IsEmpty())
		{
			return false;
		}
		auto leftIndices = this->MergeIndices();
		auto rightIndices = matcher.MergeIndices();

		if (leftIndices.size() != rightIndices.size())
		{
			return false;
		}
		for (unsigned int i = 0, count = leftIndices.size(); i < count; ++i)
		{
			if (leftIndices[i] != rightIndices[i])
			{
				return false;
			}
		}
		return true;
	}

	auto Matcher::OnEntityAdded() -> const TriggerOnEvent*
	{
		return new TriggerOnEvent(*this, GroupEventType::OnEntityAdded);
	}

	auto Matcher::OnEntityRemoved() -> const TriggerOnEvent*
	{
		return new TriggerOnEvent(*this, GroupEventType::OnEntityRemoved);
	}

	auto Matcher::OnEntityAddedOrRemoved() -> const TriggerOnEvent*
	{
		return new TriggerOnEvent(*this, GroupEventType::OnEntityAddedOrRemoved);
	}

	bool Matcher::operator ==(const Matcher right) const
	{
		return this->GetHashCode() == right.GetHashCode() && this->CompareIndices(right);
	}

	auto Matcher::MergeIndices() const -> ComponentIdList
	{
		auto indicesList = ComponentIdList();
		indicesList.reserve(mAllOfIndices.size() + mAnyOfIndices.size() + mNoneOfIndices.size());

		for (const auto &id : mAllOfIndices)
		{
			indicesList.push_back(id);
		}
		for (const auto &id : mAnyOfIndices)
		{
			indicesList.push_back(id);
		}
		for (const auto &id : mNoneOfIndices)
		{
			indicesList.push_back(id);
		}
		return DistinctIndices(indicesList);
	}

	void Matcher::CalculateHash()
	{
		unsigned int hash = typeid(Matcher).hash_code();

		hash = ApplyHash(hash, mAllOfIndices, 3, 53);
		hash = ApplyHash(hash, mAnyOfIndices, 307, 367);
		hash = ApplyHash(hash, mNoneOfIndices, 647, 683);

		mCachedHash = hash;
	}

	auto Matcher::ApplyHash(unsigned int hash, const ComponentIdList indices, int i1, int i2) const -> unsigned int
	{
		if (indices.size() > 0)
		{
			for (int i = 0, indicesLength = indices.size(); i < indicesLength; i++)
			{
				hash ^= indices[i] * i1;
			}
			hash ^= indices.size() * i2;
		}
		return hash;
	}

	auto Matcher::MergeIndices(MatcherList matchers) -> ComponentIdList
	{
		unsigned int totalIndices = 0;

		for (auto &matcher : matchers)
		{
			totalIndices += matcher.GetIndices().size();
		}

		auto indices = ComponentIdList();
		indices.reserve(totalIndices);

		for (auto &matcher : matchers)
		{
			for (const auto &id : matcher.GetIndices())
			{
				indices.push_back(id);
			}
		}
		return indices;
	}

	auto Matcher::DistinctIndices(ComponentIdList indices) -> ComponentIdList
	{
		std::sort(indices.begin(), indices.end());
		indices.erase(std::unique(indices.begin(), indices.end()), indices.end());

		return indices;

		// Old Code (delete!)
		/*auto indicesSet = unordered_set<unsigned int>(indices.begin(), indices.end());

		auto uniqueIndices = ComponentIdList();
		uniqueIndices.reserve(indicesSet.size());

		for(const auto &id : indicesSet)
		{
			uniqueIndices.push_back(id);
		}

		std::sort(uniqueIndices.begin(), uniqueIndices.end(), [](unsigned int a, unsigned int b) {
			return b < a;
		});

		return uniqueIndices;*/
	}
}