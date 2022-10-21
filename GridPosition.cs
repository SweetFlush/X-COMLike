

/// <summary>
/// x값과 z값을 담은 구조체
/// </summary>
public struct GridPosition
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return (a.x == b.x) && (a.z == b.z);
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
}
