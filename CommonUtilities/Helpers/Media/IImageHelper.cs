using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.Media;

public interface IImageHelper
{
    string ValidatePhoto(IFormFile f);
    string SavePhoto(IFormFile f, string folder);
    void DeletePhoto(string file, string folder);
}