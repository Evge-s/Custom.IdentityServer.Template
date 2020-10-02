
namespace IFTurist.Models.DataModels
{
    public class SampleData
    {
        // for seeding data future funcional
        public static void Initialize(DataContext context)
        {
            context.SaveChanges();
        }
    }
}
