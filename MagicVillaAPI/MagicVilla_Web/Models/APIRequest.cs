using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.StaticDefinitions;

namespace MagicVilla_Web.Models;

public class APIRequest
{
    public APIType APIType { get; set; } = APIType.GET;

    public string URL { get; set; }

    public object Datos {  get; set; }
}
