namespace BaseLibS.Api{
    public interface IGroupDataProvider{
        string[] CategoryNames { get; }
        int[][] GetGroupData(int ind, out int ngroups);
    }
}