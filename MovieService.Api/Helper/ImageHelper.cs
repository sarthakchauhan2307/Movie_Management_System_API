using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MovieService.Api.Helper
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _env;

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string? SaveImage(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            // ✅ correct wwwroot path
            var path = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/{folder}/{fileName}";
        }
    }
}
