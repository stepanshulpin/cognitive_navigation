using System;
using NUnit.Framework;

namespace Tests.Utils {
    public class ExtendedHeapTests {
        [Test]
        public void CanCreateExtendedHeap() {
            ExtendedHeap<int, int> heap = new ExtendedHeap<int, int>(10);
            Assert.NotNull(heap);
        }

        [Test]
        public void CanFindItemInExtendedHeap() {
            ExtendedHeap<int, int> heap = new ExtendedHeap<int, int>(10);
            heap.Push(4, 10);
            heap.Push(1, 100);
            heap.Push(31, 2);
            heap.Push(145, 5);
            HeapKeyValuePair<int, int> item;
            int heapIndex;
            heap.Find(2, out item, out heapIndex);
            Assert.AreEqual(31, item.Key);
            Assert.AreEqual(2, item.Value);
            Assert.AreEqual(2, heapIndex);
        }

        [Test]
        public void CanIncreaseKeyInExtendedHeap() {
            ExtendedHeap<TestKey, int> heap = new ExtendedHeap<TestKey, int>(10);
            heap.Push(new TestKey(4), 10);
            heap.Push(new TestKey(1), 100);
            heap.Push(new TestKey(31), 2);
            heap.Push(new TestKey(146), 5);
            HeapKeyValuePair<TestKey, int> item;
            int heapIndex;
            heap.Find(2, out item, out heapIndex);
            item.Key.Priority = 0;
            heap.KeyIncreased(heapIndex);
            Assert.AreEqual(2, heap.Peek());
        }

        private class TestKey : IComparable<TestKey> {
            public int Priority {
                get {
                    return this.priority;
                }
                set {
                    this.priority = value;
                }
            }

            public TestKey(int priority) {
                this.priority = priority;
            }

            public int CompareTo(TestKey other) {
                return this.priority.CompareTo(other.priority);
            }

            private int priority;
        }
    }
}
