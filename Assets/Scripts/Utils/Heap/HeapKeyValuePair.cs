using System;

public class HeapKeyValuePair<T1, T2> where T1 : IComparable<T1> {
    public T1 Key {
        get {
            return this.key;
        }
    }

    public T2 Value {
        get {
            return this.value;
        }
    }

    public HeapKeyValuePair(T1 key, T2 value) {
        this.key = key;
        this.value = value;
    }

    private T1 key;

    private T2 value;
}

