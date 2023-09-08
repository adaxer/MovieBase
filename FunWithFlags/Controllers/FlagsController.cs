using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FunWithFlags.Controllers;
[ApiController]
[Route("[controller]")]
public class FlagsController : ControllerBase
{
    private readonly ILogger<FlagsController> _logger;

    public FlagsController(ILogger<FlagsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("[action]/{key}", Name = "GetFlag")]
    public async Task<IActionResult> Get(string key)
    {
        try
        {
            var fileName = Path.Combine("Assets", $"{key}.png");
            if (System.IO.File.Exists(fileName))    // In Assets vorhanden?
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(fileName);
                return File(bytes, MediaTypeNames.Application.Octet, key+".png");
            }
            else    // Auf flags.cdn vorhanden?
            {
                var url = $"https://flagcdn.com/160x120/{key}.png";
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(url);
                return File(bytes, MediaTypeNames.Application.Octet, key + ".png");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No Fun");
            throw;
        }
    }
}
