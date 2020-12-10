namespace DbTools.Service
{
    public interface ITemplateService
    {
        string Render(string template, object model);
    }
}