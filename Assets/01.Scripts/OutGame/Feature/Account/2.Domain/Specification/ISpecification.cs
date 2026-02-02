public interface ISpecification<in T>
{
    ValidationResult IsSatisfiedBy(T value);
}
