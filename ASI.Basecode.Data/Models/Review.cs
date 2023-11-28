using System;

namespace ASI.Basecode.Data.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string ReviewName { get; set; }
        public int ReviewRatings { get; set; }
        public string ReviewComments { get; set; }
        public DateTime ReviewDate { get; set; }
        public int BookId { get; set; }  //foreign key 
        public BookMaster Book {  get; set; }   //navigation 
    }
}
