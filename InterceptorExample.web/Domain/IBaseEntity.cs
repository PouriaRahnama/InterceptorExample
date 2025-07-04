namespace InterceptorExample.web.Domain
{
    public interface IBaseEntity:ICreatedEntity
    {
    }

    public interface ICreatedEntity
    {
    }
    public interface ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
