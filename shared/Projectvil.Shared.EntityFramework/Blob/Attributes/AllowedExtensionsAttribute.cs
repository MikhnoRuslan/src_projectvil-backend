using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Projectvil.Shared.EntityFramework.Blob.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AllowedExtensionsAttribute : ActionFilterAttribute
{
    private List<string> AllowedExtensions { get; set; }

    public AllowedExtensionsAttribute(string fileExtensions)
    {
        AllowedExtensions = fileExtensions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var file = context.ActionArguments["file"] as IFormFile;
        
        if (file != null)
        {
            var indexOfLastDot = file.FileName.LastIndexOf('.');
            if (indexOfLastDot != -1)
            {
                var format = file.FileName.Substring(indexOfLastDot + 1);
                if (!AllowedExtensions.Contains(format, StringComparer.OrdinalIgnoreCase))
                {
                    context.Result = new BadRequestObjectResult("This file extension is not allowed");
                }
            }
        }
        else
        {
            context.Result = new BadRequestObjectResult("Invalid file");
        }
    }
}