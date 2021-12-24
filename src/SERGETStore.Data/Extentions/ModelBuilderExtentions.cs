using Microsoft.EntityFrameworkCore;

namespace SERGETStore.Data.Extentions;

public static class ModelBuilderExtentions
{
    public static void DesabilitarCascadingDelete(this ModelBuilder modelBuilder)
    {
        var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var FK in foreignKeys)
        {
            FK.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }
    }
}
