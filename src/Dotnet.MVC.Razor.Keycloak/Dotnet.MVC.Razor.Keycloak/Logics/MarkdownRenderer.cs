namespace Dotnet.MVC.Razor.Keycloak.Logics;

public class MarkdownRenderer : IMarkdownRenderer
{
    public string ToHtml(string markdown) => Markdig.Markdown.ToHtml(markdown ?? "");
}
