using System;

namespace KataokaLib.ValueObject {

    public class ValueObjectExceptionHandler {

        public static void ThrowsWHenInValidValue<T>(
            T value,
            T MIN,
            T MAX,
            Exception exception
        ) {
            if ((dynamic)value < (dynamic)MIN || (dynamic)value > (dynamic)MAX) throw exception;
        }

        public static void ThrowsWHenInValidValue<T>(
            T start,
            T end,
            T MIN,
            T MAX,
            Exception exception
        ) {
            if ((dynamic)start < (dynamic)MIN || (dynamic)end > (dynamic)MAX) throw exception;
        }

        public static Exception ArgumentException(string param) {
            return new ArgumentException(
                ValueObjectExceptionMessage.argumentExceptionMessage,
                param
            );
        }

        public static Exception InvalidOperationExceptionOfClassEquals() {
            return new InvalidOperationException(
                ValueObjectExceptionMessage.InvalidOperationExceptionOfClassEqualsMessage
            );
        }

    }

}
