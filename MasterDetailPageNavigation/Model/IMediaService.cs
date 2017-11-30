using System.IO;
using System.Threading.Tasks;

namespace Lebenslauf
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}