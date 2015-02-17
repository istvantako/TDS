using System.IO;
using System.Linq;

namespace Tds.Tests.Model.Entities
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Makes sure the given entity exists in the database with the desired properties.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="ctx">Entity framework context instance</param>
        public static void Ensure<T>(this T entity, TdsContext ctx)
            where T : class, ISelfEnsuringEntity<T>
        {
            ctx.EnsureEntity<T>(entity, entity.GetCheckIfExistsExpression());
        }

        /// <summary>
        /// Gets the storage file name of the given entity, combined with the storage folder path if specified.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="storageFolder">Path to storage folder (optional)</param>
        /// <returns></returns>
        public static string GetExpectedFileName<T>(this T entity, string storageFolder = "")
            where T : class, IEntityWithIds<T>
        {
            var fileName = string.Format("{0}_{1}.xml",
                    entity.GetType().Name,
                    string.Join("_", entity.GetListOfIdSelectors().Select(x => x.Invoke(entity))));

            return string.IsNullOrWhiteSpace(storageFolder) ? fileName : Path.Combine(storageFolder, fileName);
        }
    }
}
