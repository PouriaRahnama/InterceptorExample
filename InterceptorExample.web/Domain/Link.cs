namespace InterceptorExample.web.Domain
{
    public class Link:ICreatedEntity
    {
        public Guid Id { get; set; }
        public required string shortenUrl { get; set; }
        public required string destenationUrl { get; set; }

    }
}
