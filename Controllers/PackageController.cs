using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace PackageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PackageController : ControllerBase
{
    private static ConcurrentBag<Package> packages = new ConcurrentBag<Package>();

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

        return Ok();
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
        return package.Weight <= 20000 &&
               package.Length <= 60 &&
               package.Height <= 60 &&
               package.Width <= 60;
    }
    private string GenerateUniqueKolliId()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random();
        var randomNumbers = random.Next(1, 9).ToString();
        
        return $"999{timestamp}{randomNumbers}";
    }
}
