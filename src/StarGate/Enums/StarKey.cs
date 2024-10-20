﻿namespace StarGate.Enums
{
    internal class SomeOne { }
    internal class All { }
    internal class SomeOneButNullPrefer { }
    internal class NullKey { }
    public static class StarKey
    {
        public static object SomeOne { get; } = new SomeOne();
        public static object All { get; } = new All();
        public static object SomeOneButNullPrefer { get; } = new SomeOneButNullPrefer();
        internal static object NullKey { get; } = new NullKey();
    }
}

