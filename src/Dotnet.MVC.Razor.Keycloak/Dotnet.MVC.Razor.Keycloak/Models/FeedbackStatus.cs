using System.Text.Json.Serialization;

namespace Dotnet.MVC.Razor.Keycloak.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FeedbackStatus
{
    New = 0,
    InProgress = 1,
    Processed = 2,
    NotInteresting = 3
}
