using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("PackageControllerTests")]
namespace PackageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PackageController : ControllerBase
{
    internal static ConcurrentBag<Package> packages = new ConcurrentBag<Package>();

    [HttpGet]
    public IActionResult GetPackages()
    {
        var packageList = packages.ToList();
        return Ok(packageList);
    }

    [HttpPost]
    public IActionResult CreatePackage([FromBody] Package package)
    {
        if (!IsValidPackage(package))
        {
            return BadRequest("Invalid package data.");
        }
        package.KolliId = GenerateUniqueKolliId();
        packages.Add(package);
        var message = "Kolli was successfully created";
        var response = new { Message = message, KolliId = package.KolliId };

        return Ok(response);
    }

    [HttpGet("{kolliId}")]
    public IActionResult GetPackageDetails(string kolliId)
    {
        var package = packages.FirstOrDefault(p => p.KolliId == kolliId);
        if (package == null)
        {
            return NotFound($"Package with KolliId '{kolliId}' not found.");
        }

        return Ok(package);
    }
    private bool IsValidPackage(Package package)
    {
        return package.Weight > 0 &&
           package.Length > 0 &&
           package.Height > 0 &&
           package.Width > 0 &&
           package.Weight <= 20000 &&
           package.Length <= 60 &&
           package.Height <= 60 &&
           package.Width <= 60;
    }
    private string GenerateUniqueKolliId()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random();
        var randomNumber = random.Next(1, 9).ToString();
        
        return $"999{timestamp}{randomNumber}";
    }
}
