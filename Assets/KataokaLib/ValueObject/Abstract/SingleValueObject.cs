// ! c# 11 のジェネリクス同士の演算に対応できたら、対応したい
// * dynamicを使用した演算では、演算結果が未知数なことが多いため

using System;

namespace KataokaLib.ValueObject {

    public abstract class SingleValueObject<T> :
        ISingleValueObject<T>,
        IComparable<SingleValueObject<T>>
    {

        public T Value { get; }
        public T MIN { get; }
        public T MAX { get; }

        protected SingleValueObject(T value, T MIN, T MAX) {
            this.MIN = MIN;
            this.MAX = MAX;

            ValueObjectExceptionHandler.ThrowsWHenInValidValue(
                value,
                MIN,
                MAX,
                ValueObjectExceptionHandler.ArgumentException(nameof(value))
            );

            Value = value;
        }

        public override string ToString() {
            return $"{Value}";
        }

        public override int GetHashCode() {
            return (Value, MIN, MAX).GetHashCode();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is SingleValueObject<T> other && Equals(other);
        }

        public bool Equals(SingleValueObject<T> other) {
            if (!ClassEquals(this, other))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();
            return this.GetHashCode() == other.GetHashCode();
        }

        public int CompareTo(SingleValueObject<T> other) {
            if ((dynamic)Value > (dynamic)other.Value) return 1;
            if ((dynamic)Value < (dynamic)other.Value) return -1;
            return 0;
        }

        public static bool ClassEquals(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            return lh.GetType().Name.Equals(rh.GetType().Name);
        }

        public static bool operator==(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            return lh.Equals(rh);
        }

        public static bool operator!=(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            return !(lh == rh);
        }

        public static bool operator<(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();

            return (dynamic)lh.Value < (dynamic)rh.Value;
        }

        public static bool operator>(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();

            return ClassEquals(lh, rh) && (dynamic)lh.Value > (dynamic)rh.Value;
        }

        public static bool operator<=(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();

            return ClassEquals(lh, rh) && (dynamic)lh.Value <= (dynamic)rh.Value;
        }

        public static bool operator>=(SingleValueObject<T> lh, SingleValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();

            return ClassEquals(lh, rh) && (dynamic)lh.Value >= (dynamic)rh.Value;
        }

    }

}
