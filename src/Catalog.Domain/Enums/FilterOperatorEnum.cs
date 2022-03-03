namespace Catalog.Domain.Enums
{
    public enum FilterOperatorEnum
    {
        IsEqualTo = 0,
        IsNotEqualTo = 1,
        StartsWith = 2,
        Contains = 3,
        DoesNotContain = 4,
        EndsWith = 5,
        IsNull = 6,
        IsNotNull = 7,
        IsEmpty = 8,
        IsNotEmpty = 9,
        HasNoValue = 10,
        HasValue = 11,
        IsEqualToGuid = 12,
        IsEqualToNotNullGuid = 13,
        Between = 14,
        GreaterThan = 15,
        GreaterThanOrEqual = 16
    }
}
