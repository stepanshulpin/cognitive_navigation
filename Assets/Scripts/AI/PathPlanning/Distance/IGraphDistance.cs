namespace AI.PathPlanning {
    public interface IGraphDistance<T> {
        T Calculate(INavigationNode nodeA, INavigationNode nodeB);
    }
}
