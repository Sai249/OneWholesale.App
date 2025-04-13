using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using OneWholesale.Model.Models;
using OneWholesale.Repository.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.IO;

namespace OneWholesale.App.Controllers
{
    [Route("[controller]")]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<BrandController> _logger;

        public BrandController(
            IBrandRepository brandRepository,
            IWebHostEnvironment hostingEnvironment,
            ILogger<BrandController> logger)
        {
            _brandRepository = brandRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var model = new Brand();
            return View("~/Views/Brand/Create.cshtml", model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] Brand model, [FromForm] IFormFile? CompanyLogo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? logoPath = CompanyLogo != null ? SaveCompanyLogo(CompanyLogo) : null;
                    model.CompanyLogo = logoPath;

                    bool result = _brandRepository.ManageBrand("Insert", model, "Admin");

                    if (result)
                        return Ok(new { success = true, message = "Brand created successfully!" });

                    return BadRequest(new { success = false, message = "Failed to create brand." });
                }

                return BadRequest(new { success = false, message = "Invalid data." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during brand creation.");
                return StatusCode(500, new { success = false, message = "Server error during brand creation." });
            }
        }

        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBrand([FromForm] Brand model, [FromForm] IFormFile? CompanyLogo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? logoPath = CompanyLogo != null ? SaveCompanyLogo(CompanyLogo) : model.CompanyLogo;
                    model.CompanyLogo = logoPath;

                    bool result = _brandRepository.ManageBrand("Update", model, "Admin");

                    if (result)
                        return Ok(new { success = true, message = "Brand updated successfully!" });

                    return BadRequest(new { success = false, message = "Failed to update brand." });
                }

                return BadRequest(new { success = false, message = "Invalid data." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during brand update.");
                return StatusCode(500, new { success = false, message = "Server error during brand update." });
            }
        }


        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var brand = _brandRepository.GetBrandById(id);
            if (brand == null)
                return NotFound();

            return View("Update", brand); 
        }

      
        [HttpGet("BrandList")]
        public IActionResult BrandList()
        {
            var brandList = _brandRepository.GetAllBrands();
            return View("BrandList", brandList);
        }


        [HttpPost("DeleteBrand/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            try
            {
                bool success = _brandRepository.DeleteBrand(id, "Admin");

                if (success)
                    return Json(new { success = true, message = "Brand deleted successfully" });

                return Json(new { success = false, message = "Failed to delete brand" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete failed.");
                return Json(new { success = false, message = "An error occurred during deletion." });
            }
        }

        private string? SaveCompanyLogo(IFormFile? companyLogo)
        {
            if (companyLogo != null && companyLogo.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(companyLogo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    companyLogo.CopyTo(stream);
                }

                return "/uploads/" + uniqueFileName;
            }

            return null;
        }
    }
}
