using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECraft.Models
{
	public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
	{
		public TKey Id { get; set; }

		public static bool operator ==(BaseEntity<TKey> left, BaseEntity<TKey> right)
		{
			if (ReferenceEquals(left, right)) return true;

			if (left is null || right is null) return false;

			return left.Id.Equals(right.Id);
		}

		public static bool operator !=(BaseEntity<TKey> left, BaseEntity<TKey> right)
		{
			if (ReferenceEquals(left, right)) return false;

			if (left is null || right is null) return true;

			return !left.Id.Equals(right.Id);
		}

		public override bool Equals(object? obj)
		{
			if (obj is BaseEntity<TKey>)
			{
				return this == (BaseEntity<TKey>)obj;
			}

			return false;

		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}
	}
}
