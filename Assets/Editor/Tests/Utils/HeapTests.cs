using NUnit.Framework;

namespace Tests.Utils {
    public class HeapTests {
        [Test]
        public void CanCreateHeap() {
            Heap<int, int> heap = new Heap<int, int>(10);
            Assert.NotNull(heap);
        }

        [Test]
        public void WhenItemAddedToHeapItemsCountIsIncreasedByOne() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(3, 5);
            Assert.AreEqual(1, heap.Count);
        }

        [Test]
        public void WhenItemAddedToHeapPeekReturnsThisItem() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(1, 5);
            Assert.AreEqual(5, heap.Peek());
        }

        [Test]
        public void WhenTwoItemsAddedToHeapPeekReturnsItemWithLowestPriority() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(4, 5);
            heap.Push(1, 10);
            Assert.AreEqual(10, heap.Peek());
        }

        [Test]
        public void WhenOneItemAddedToHeapPopReturnThisItem() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(4, 134);
            Assert.AreEqual(134, heap.Pop());
        }

        [Test]
        public void WhenHeapContainsOneItemCountIsDecreasedByOneAfterPop() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(5, 1);
            heap.Pop();
            Assert.AreEqual(0, heap.Count);
        }

        [Test]
        public void WhenItemWithLowestPriorityArePopedYouCanPeekItemWithSecodLowestPriority() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(4, 1);
            heap.Push(1, 43);
            Assert.AreEqual(43, heap.Pop());
            Assert.AreEqual(1, heap.Pop());
        }

        [Test]
        public void CanGetCorrectItemAfterPushPopSequences() {
            Heap<int, int> heap = new Heap<int, int>(10);
            heap.Push(2, 5);
            heap.Push(10, 10);
            Assert.AreEqual(5, heap.Pop());
            heap.Push(6, 3);
            Assert.AreEqual(3, heap.Pop());
            heap.Push(12, 11);
            Assert.AreEqual(10, heap.Pop());
        }
    }
}
