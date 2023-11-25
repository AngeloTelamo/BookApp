using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        public ReviewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
        public bool ReviewUserExists(string reviewName)
        {
            return this.GetDbSet<Review>().Any(x => x.ReviewName == reviewName);
        }

        public void AddReview(Review review)
        {
            this.GetDbSet<Review>().Add(review);
            UnitOfWork.SaveChanges();
        }
    }
}
