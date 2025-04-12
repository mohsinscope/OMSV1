namespace OMSV1.Domain.Enums
{
    public enum DocumentActions
    {
        Add     = 1,
        Edit    = 2,
        Reply   = 3,
        Confirm = 4,
        ChangeStatus=5, // when ISrequireReply is false seperate endpoint and is done by abdullah
    }
}
