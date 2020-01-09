namespace BaseLibS.Api{
    public interface INamedListItem : INamedItem{
        /// <summary>
        /// This number controls the order in which items are displayed e.g. in drop down menus.
        /// </summary>
        float DisplayRank { get; }

        /// <summary>
        /// If false is returned, the item will not be available.
        /// </summary>
        bool IsActive { get; }
    }
}