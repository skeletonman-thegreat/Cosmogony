public class UnionFind
{
    private int[] parent;

    public UnionFind(int Size)
    {
        parent = new int[Size];
        for(int i = 0; i < Size; i++)
        {
            parent[i] = i; //each node has its own parent initially
        }
    }

    public int Find(int i)
    {
        if (parent[i] != i) parent[i] = Find(parent[i]);
        return parent[i];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        if(rootX != rootY) parent[rootX] = rootY;
    }
}