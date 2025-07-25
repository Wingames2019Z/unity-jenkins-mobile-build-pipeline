public partial class ProjectTypeEnum : PartialEnum<ProjectTypeEnum>
{
    public static ProjectTypeEnum DEVELOP { get { return GetValue("DEVELOP"); } }
    public static ProjectTypeEnum RELEASE { get { return GetValue("RELEASE"); } }
    public static ProjectTypeEnum STAGING { get { return GetValue("STAGING"); } }
}
