using System;

namespace KataokaLib.ValueObject {

    public abstract class RangeValueObject<T> :
        IRangeValueObject<T>,
        IComparable<RangeValueObject<T>>
    {

        public T Start { get; }
        public T End { get; }
        public T Size { get; }
        public T MIN { get; }
        public T MAX { get; }

        protected RangeValueObject(T start, T end, T MIN, T MAX) {
            this.MIN = MIN;
            this.MAX = MAX;

            ValueObjectExceptionHandler.ThrowsWHenInValidValue(
                start,
                end,
                MIN,
                MAX,
                ValueObjectExceptionHandler.ArgumentException(nameof(start) + ", " + nameof(end))
            );

            Start = start;
            End = end;
            Size = (dynamic)End - (dynamic)Start;
        }

        public override string ToString() {
            return $"{Start}, {End}";
        }

        public override int GetHashCode() {
            return (Start, End, MIN, MAX).GetHashCode();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is RangeValueObject<T> other && Equals(other);
        }

        public bool Equals(RangeValueObject<T> other) {
            if (!ClassEquals(this, other))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();
            return this.GetHashCode() == other.GetHashCode();
        }

        public int CompareTo(RangeValueObject<T> other) {
            if ((dynamic)Size > (dynamic)other.Size) return 1;
            if ((dynamic)Size < (dynamic)other.Size) return -1;
            return 0;
        }

        public static bool ClassEquals(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            return lh.GetType().Name.Equals(rh.GetType().Name);
        }

        public static bool operator==(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            return lh.Equals(rh);
        }

        public static bool operator!=(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            return !(lh == rh);
        }

        public static bool operator<(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();
            
            T lhLength = (dynamic)lh.End - (dynamic)lh.Start;
            T rhLength = (dynamic)rh.End - (dynamic)rh.Start;
            return (dynamic)lhLength < (dynamic)rhLength;
        }

        public static bool operator>(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();

            T lhLength = (dynamic)lh.End - (dynamic)lh.Start;
            T rhLength = (dynamic)rh.End - (dynamic)rh.Start;
            return (dynamic)lhLength > (dynamic)rhLength;
        }

        public static bool operator<=(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();
            
            T lhLength = (dynamic)lh.End - (dynamic)lh.Start;
            T rhLength = (dynamic)rh.End - (dynamic)rh.Start;
            return (dynamic)lhLength <= (dynamic)rhLength;
        }

        public static bool operator>=(RangeValueObject<T> lh, RangeValueObject<T> rh) {
            if (!ClassEquals(lh, rh))
                throw ValueObjectExceptionHandler.InvalidOperationExceptionOfClassEquals();
            
            T lhLength = (dynamic)lh.End - (dynamic)lh.Start;
            T rhLength = (dynamic)rh.End - (dynamic)rh.Start;
            return (dynamic)lhLength >= (dynamic)rhLength;
        }

    }

}