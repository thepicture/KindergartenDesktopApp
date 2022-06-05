namespace KindergartenDesktopApp
{
    /// <summary>
    /// Implements static extensions for Entity Framework entities.
    /// </summary>
    public static class EntitiesExtensions
    {
        /// <summary>
        /// Determines whether entity's Id field is 0. 
        /// Use only for Entity Framework entities.
        /// </summary>
        /// <param name="entity">Entity to determine id from.</param>
        /// <returns><see langword="true"/> if and only if entity.Id == 0.</returns>
        public static bool IsNew(this object entity)
        {
            return (int)entity.GetType().GetProperty("Id").GetValue(entity) == 0;
        }
    }
}
