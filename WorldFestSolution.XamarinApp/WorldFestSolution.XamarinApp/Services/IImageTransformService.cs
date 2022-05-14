namespace WorldFestSolution.XamarinApp.Services
{
    public interface IImageTransformService
    {
        byte[] Transform(byte[] imageData,
                         float width,
                         float height,
                         int quality);
    }
}