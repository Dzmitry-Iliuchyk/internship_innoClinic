using System;

namespace Shared.Abstractions.Entities {
    public abstract class Entity<TIdentifier> {
        public TIdentifier Id { get; set; }

    }
    public class PagedResult<T> {
        public PagedResult( int totalCount, IList<T> items ) {
            this.TotalCount = totalCount;
            this.Items = items;
        }
        public PagedResult() {
        }

        public int TotalCount { get; set; }
        public IList<T> Items { get; set; } = new List<T>();
    }

}
