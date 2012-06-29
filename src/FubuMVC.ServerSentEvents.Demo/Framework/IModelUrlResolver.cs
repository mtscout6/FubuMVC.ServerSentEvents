namespace FubuMVC.ServerSentEvents.Demo.Framework
{
    public interface IModelUrlResolver
    {
        string GetUrlForInputModelName(string modelTypeName);
    }
}