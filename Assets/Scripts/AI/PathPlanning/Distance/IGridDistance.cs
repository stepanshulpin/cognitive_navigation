namespace AI.PathPlanning {
    public interface IGridDistance {
        int Calculate(INavigationGridNode nodeA, INavigationGridNode nodeB);
    }
}
