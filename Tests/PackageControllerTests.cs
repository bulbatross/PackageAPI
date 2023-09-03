using Microsoft.AspNetCore.Mvc;
using Moq;
using PackageAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Xunit;

public class PackageControllerTests
{
    [Fact]
    public void GetPackages_ReturnsOkResultWithPackageList()
    {
        var packages = new List<Package> { new Package { Weight = 14000, 
            Length = 30,   
            Height = 30, 
            Width = 60 } };
        var controller = new PackageController();
        SetupController(packages);

        var result = controller.GetPackages() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var packageList = result.Value as List<Package>;
        Assert.NotNull(packageList);
        Assert.Single(packageList);
    }

    [Fact]
    public void CreatePackage_WithValidPackageData_ReturnsOkResultWithMessageAndKolliId()
    {
        var controller = new PackageController();
        var package = new Package {
            Weight = 14000,
            Length = 30,
            Height = 30,
            Width = 60
        };

        var result = controller.CreatePackage(package) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var response = result.Value as dynamic;
        Assert.NotNull(response);
        Assert.Equal("Kolli was successfully created", response.Message);
        Assert.NotNull(response.KolliId);
    }

    [Fact]
    public void CreatePackage_WithInvalidPackageData_ReturnsBadRequestWithMessage()
    {
        var controller = new PackageController();
        var package = new Package
        {
            Weight = 22000,
            Length = 100,
            Height = 130,
            Width = 70
        };

        var result = controller.CreatePackage(package) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        var errorMessage = result.Value as string;
        Assert.NotNull(errorMessage);
        Assert.Equal("Invalid package data.", errorMessage);
    }

    [Fact]
    public void GetPackageDetails_WithValidKolliId_ReturnsOkResultWithPackage()
    {
        var packages = new List<Package>
        {
            new Package { KolliId = "999123456789012345", 
                Weight = 20000,
                Length = 60,
                Height = 50,
                Width = 20 }
        };

        var controller = new PackageController();
        SetupController(packages);

        var validKolliId = "999123456789012345"; // Ett giltigt KolliId som finns i listan.

        var result = controller.GetPackageDetails(validKolliId) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var package = result.Value as Package;
        Assert.NotNull(package);
        Assert.Equal(validKolliId, package.KolliId);
    }

    [Fact]
    public void GetPackageDetails_WithInvalidKolliId_ReturnsNotFoundResult()
    {
        var packages = new List<Package>
        {
            new Package { KolliId = "999123456789012345",
                Weight = 20000,
                Length = 60,
                Height = 50,
                Width = 20 }
        };

        var controller = new PackageController();
        SetupController(packages);

        var invalidKolliId = "invalid"; // Ett ogiltigt KolliId som inte finns i listan.

        var result = controller.GetPackageDetails(invalidKolliId) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        var errorMessage = result.Value as string;
        Assert.NotNull(errorMessage);
        Assert.Equal(errorMessage, $"Package with KolliId '{invalidKolliId}' not found.");
    }

    private void SetupController(List<Package> packages)
    {
        PackageController.packages = new ConcurrentBag<Package>(packages);
    }
}
