namespace WorldFestSolution.ImportApp
{
    public interface IImageTransformService
    {
        byte[] Transform(byte[] imageData, float width, float height, int quality);
    }
}