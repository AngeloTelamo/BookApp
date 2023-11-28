using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class BookGenreMaster
    {
        [Key]
        public int GenreId { get; set; }
        public string GenreName { get; set;}

        public ICollection<BookMaster> Books { get; set; }
    }
}
