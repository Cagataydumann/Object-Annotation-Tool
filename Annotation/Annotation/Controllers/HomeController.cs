using Annotation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Annotation.Controllers
{
    public class HomeController : Controller
    {
        private readonly AnnotationDbContext _context;

        public HomeController(AnnotationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            List<Class> classes = await _context.Classes.ToListAsync();
            return PartialView("_ClassesPartial", classes);
        }

        [HttpGet]
        public IActionResult GetBoundingBoxes()
        {
            string filePath = "annotation.txt";
            string[] lines = System.IO.File.ReadAllLines(filePath);
            return PartialView("_BoundingBoxesPartial", lines);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> SaveAnnotations([FromBody] List<BoundingBox> annotations)
        {
            Console.WriteLine("Received annotations:");
            Console.WriteLine(JsonConvert.SerializeObject(annotations));
            try
            {
                if (annotations == null || annotations.Count == 0)
                {
                    string errorMessage = "Etiketler boş.";
                    Console.WriteLine(errorMessage);

                    return BadRequest(new { error = errorMessage });
                }

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "annotation.txt");
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath).Close();
                }

                object fileLock = new object(); 

                using (StreamWriter writer = new StreamWriter(filePath, append: true, encoding: Encoding.UTF8, bufferSize: 1024))
                {
                    foreach (var annotation in annotations)
                    {
                        if (annotation.ClassName != null)
                        {
                            await writer.WriteLineAsync($"{annotation.X},{annotation.Y},{annotation.Width},{annotation.Height},{annotation.ClassName}");
                        }
                    }
                }

                foreach (var annotation in annotations)
                {
                    var className = annotation.ClassName;
                    var classEntity = _context.Classes.FirstOrDefault(c => c.ClassName == className);
                    if (classEntity != null)
                    {
                        var boundingBox = new BoundingBox
                        {
                            X = annotation.X,
                            Y = annotation.Y,
                            Width = annotation.Width,
                            Height = annotation.Height,
                            ClassName = annotation.ClassName,
                            ClassId = classEntity.Id
                        };
                        _context.BoundingBoxes.Add(boundingBox);
                    }
                }

                await _context.SaveChangesAsync();

                return new JsonResult(new { message = "Etiketler başarıyla kaydedildi." })
                {
                    StatusCode = 200,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                string errorMessage = $"Bir hata oluştu. İşlem başarısız oldu. Hata: {ex.ToString()}";
                Console.WriteLine(errorMessage);

                return StatusCode(500, new { error = errorMessage });
            }
        }
    }
    }
