using System.Collections.Generic;
using System.Linq;

namespace Sooda.UnitTests.BaseObjects.Interfaces
{
    /// <summary>
    /// vehicle mileage
    /// </summary>
    public interface IMileage
    {
        int Total { get; }

        void registerMileage(int mileage);

        IEnumerable<IMileageItem> Items { get; }
        IQueryable<IMileageItem> ItemsQuery { get; }
    }
}