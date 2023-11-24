using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace SD.Editor.Controllers
{
    public class GenerateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateNewDocument(string newContent)
        {
            // Ruta al documento de Word existente.
            string filePath = "Plantillas/Ultrasonido.docx";

            // Cargar el documento.
            var document = DocumentModel.Load(filePath);

            // Crear un nuevo párrafo con el nuevo contenido.
            var newParagraph = new Paragraph(document, newContent);

            // Obtener la última sección del documento.
            var lastSection = document.Sections.LastOrDefault();

            // Verificar si hay secciones en el documento.
            if (lastSection != null)
            {
                lastSection.Blocks.Add(newParagraph);
            }
            else
            {
                // Si no hay secciones en el documento, agregar una nueva sección con el nuevo párrafo.
                document.Sections.Add(new Section(document, newParagraph));
            }

            // Guardar el documento en formato DOCX en memoria.
            var stream = new System.IO.MemoryStream();
            var stream2 = new System.IO.MemoryStream();

            document.Save(stream, SaveOptions.DocxDefault);
            document.Save(stream2, SaveOptions.HtmlDefault);

            // Guardar la ruta del nuevo documento en ViewBag.
            ViewBag.NewDocumentPath = "nuevoUltrasonido.docx";

            // Devolver el archivo para su descarga.
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "nuevoUltrasonido.docx");
        }

        private string GetDocumentContent(DocumentModel document)
        {
            // Leer e imprimir el contenido del documento.
            string content = string.Empty;
            foreach (var paragraph in document.GetChildElements(true, ElementType.Paragraph))
            {
                content += paragraph.ToString() + "<br/>";
            }

            return content;
        }
    }
}
