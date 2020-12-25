namespace Net5SerilogEFCore3D.Model.DomainCoreModels
{
    public enum DomainHandlerType
    {
        Register = 1,
        Update = 2, 
        Remove = 3,
        SoftDelete = 4,
        SoftResume = 5,
        Reply = 6,
        Validation = 7, 
        Commit = 8, 
        Retrieve = 9
    }
}
