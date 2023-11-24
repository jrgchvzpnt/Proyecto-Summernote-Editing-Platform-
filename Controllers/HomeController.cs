using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using HtmlAgilityPack;
using System.Text;


namespace SD.Editor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            ComponentInfo.SetLicense("KEY PRIVADA");

        }

        public IActionResult Index()
        {
            return View();
        }


    
        private string ConvertToHtml(DocumentModel document)
        {
          

            // Crea un flujo de memoria para almacenar el contenido HTML
            using (var stream = new MemoryStream())
            {
                // Guarda el documento en formato HTML en el flujo de memoria
                document.Save(stream, SaveOptions.HtmlDefault);

                // Lee el contenido HTML del flujo de memoria
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [HttpPost]
        public IActionResult UploadWordFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Verificar la extensión del archivo
                    var allowedExtensions = new[] { ".html", ".docx" };
                    var fileExtension = Path.GetExtension(file.FileName);

                    if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        // Archivo no permitido
                        throw new NotSupportedException("Tipo de archivo no permitido.");
                    }

                    // Lee el contenido del archivo Word y conviértelo a HTML
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        var document = DocumentModel.Load(stream);
                        string htmlContent = ConvertToHtml(document);

                        return Content(htmlContent, "text/html");
                    }
                }

                return BadRequest("No se ha proporcionado un archivo válido.");
            }
            catch (NotSupportedException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error inesperado: {ex.Message}");
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

     
    }
}
