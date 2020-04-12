using System;
using System.Collections.Generic;

public class ExtendedHeap<T1, T2> : Heap<T1, T2> where T1 : IComparable<T1> where T2 : IEquatable<T2> {
    public ExtendedHeap(int maxSize) : base(maxSize) {
    }

    public void KeyIncreased(int index) {
        this.SiftUp(index);
    }

    public bool Find(T2 item, out HeapKeyValuePair<T1, T2> keyValuePair, out int index) {
        for (int i = 0; i < this.count; i++) {
            if (this.items[i].Value.Equals(item)) {
                index = i;
                keyValuePair = this.items[i];
                return true;
            }
        }
        index = -1;
        keyValuePair = null;
        return false;
    }
}
