namespace KataokaLib.ValueObject {

    public interface IRangeValueObject<T> {

        T Start { get; }
        T End { get; }
        T MIN { get; }
        T MAX { get; }

    }

}