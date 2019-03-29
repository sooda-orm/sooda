using System.Data.SqlTypes;

namespace Sooda.UnitTests.BaseObjects.Interfaces
{
    public interface IMileageItem
    {
        int Miles { get; }
        SqlString Description { get; }
    }
}