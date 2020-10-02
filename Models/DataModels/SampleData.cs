using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
