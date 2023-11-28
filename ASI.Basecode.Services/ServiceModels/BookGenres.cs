using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookGenres
    {
        public BookMasterViewModel Master { get; set; }
        public BookGenreModel Genre { get; set; }
        public int BookGenreId { get; set; } //foreign key sa bookmaster
    }
}
