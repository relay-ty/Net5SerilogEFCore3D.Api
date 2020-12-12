namespace Net5SerilogEFCore3D.Domain.Core.Notifications
{
    public enum HandlerType
    {
        Register = 1,
        Update = 2, 
        Remove = 3,
        SoftDelete = 4,
        SoftResume = 5,
        Reply = 6,
        Validation = 7, 
        Commit = 8
    }
}
