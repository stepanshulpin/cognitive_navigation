using System;

public class Heap<T1, T2> where T1 : IComparable<T1> {
    public int Count {
        get {
            return this.count;
        }
    }

    public Heap(int maxSize) {
        this.maxSize = maxSize;
        this.items = new HeapKeyValuePair<T1, T2>[maxSize];
        this.count = 0;
    }

    public virtual void Push(T1 key, T2 value) {
        if (this.count < this.maxSize) {
            this.items[this.count] = new HeapKeyValuePair<T1, T2>(key, value);
            this.SiftUp(this.count);
            this.count += 1;
        } else {
            throw new IndexOutOfRangeException("Heap is full");
        }
    }

    public virtual T2 Peek() {
        if (this.count > 0) {
            return this.items[0].Value;
        } else {
            throw new IndexOutOfRangeException("Heap is empty");
        }
    }

    public HeapKeyValuePair<T1, T2> PeekKeyValuePair() {
        if (this.count > 0) {
            return this.items[0];
        } else {
            throw new IndexOutOfRangeException("Heap is empty");
        }
    }

    public virtual T2 Pop() {
        if (this.count > 0) {
            HeapKeyValuePair<T1, T2> topItem = this.items[0];
            this.count -= 1;
            this.items[0] = this.items[this.count];
            this.SiftDown(0);
            return topItem.Value;
        } else {
            throw new IndexOutOfRangeException("Heap is empty");
        }
    }

    public HeapKeyValuePair<T1, T2> PopKeyValuePair() {
        if (this.count > 0) {
            HeapKeyValuePair<T1, T2> topItem = this.items[0];
            this.count -= 1;
            this.items[0] = this.items[this.count];
            this.SiftDown(0);
            return topItem;
        } else {
            throw new IndexOutOfRangeException("Heap is empty");
        }
    }

    protected void SiftUp(int nodeIndex) {
        HeapKeyValuePair<T1, T2> node = this.items[nodeIndex];
        int parentIndex = (nodeIndex - 1) / 2;
        HeapKeyValuePair<T1, T2> parentNode = this.items[parentIndex];
        while (node.Key.CompareTo(parentNode.Key) < 0) {
            this.items[parentIndex] = node;
            this.items[nodeIndex] = parentNode;
            nodeIndex = parentIndex;
            parentIndex = (nodeIndex - 1) / 2;
            parentNode = this.items[parentIndex];
        }
    }

    protected void SiftDown(int nodeIndex) {
        bool isSorted = false;
        int leftChildIndex = 0;
        int rightChildIndex = 0;
        int childIndexToSwap = 0;
        HeapKeyValuePair<T1, T2> node = this.items[nodeIndex];
        HeapKeyValuePair<T1, T2> childToSwap;
        while (!isSorted) {
            isSorted = true;
            leftChildIndex = nodeIndex * 2 + 1;
            rightChildIndex = nodeIndex * 2 + 2;
            if (leftChildIndex < this.count) {
                childIndexToSwap = leftChildIndex;
                if (rightChildIndex < this.count) {
                    if (this.items[leftChildIndex].Key.CompareTo(this.items[rightChildIndex].Key) > 0) {
                        childIndexToSwap = rightChildIndex;
                    }
                }
                childToSwap = this.items[childIndexToSwap];
                if (node.Key.CompareTo(childToSwap.Key) > 0) {
                    this.items[childIndexToSwap] = node;
                    this.items[nodeIndex] = childToSwap;
                    nodeIndex = childIndexToSwap;
                    isSorted = false;
                }
            }
        }
    }

    protected int count;

    protected int maxSize;

    protected HeapKeyValuePair<T1, T2>[] items;
}
