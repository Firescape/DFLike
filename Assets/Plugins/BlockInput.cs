public class BlockInput : object
{
    public Block container;
    public Point target;

    public BlockInput(Block container, Point target)
    {
        this.container = container;
        this.target = target;
    }
}