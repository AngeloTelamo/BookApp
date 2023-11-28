using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IReviewService
    {
        ReviewList GetReviewList(ReviewList model);
        void AddReviews(BookReviewsModel review);
        void DeleteReview(int reviewId);
    }
}
