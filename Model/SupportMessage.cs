

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SupportWebApp.Models;

public class SupportMessage
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [JsonProperty("category")]
    public string Category { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    public string Phone { get; set; } = "";

    [Required]
    public string Subject { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    public string Status { get; set; } = "Ny";

    public string Priority { get; set; } = "Normal";
}